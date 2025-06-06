// Copyright (c) 2019 Samsung Electronics Co., Ltd.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace Tizen.NUI.BaseComponents
{
    /// <summary>
    /// A visual view control if a user adds any visual to it.
    /// </summary>
    /// <example>
    /// Example:
    /// <code>
    /// VisualView _visualView = new VisualView();
    /// ImageVisualMap imageVisualMap1 = new ImageVisualMap();
    /// imageVisualMap1.URL = "./NUISample/res/images/image-1.jpg";
    /// imageVisualMap1.VisualSize = new Vector2( 300.0f, 300.0f );
    /// imageVisualMap1.Offset = new Vector2( 50.0f, 50.0f );
    /// imageVisualMap1.OffsetSizeMode = new Vector4( 1.0f, 1.0f, 1.0f, 1.0f );
    /// imageVisualMap1.Origin = AlignType.TOP_BEGIN;
    /// imageVisualMap1.AnchorPoint = AlignType.TOP_BEGIN;
    /// _visualView.AddVisual("imageVisual1", imageVisualMap1);
    /// </code>
    /// </example>
    /// <since_tizen> 3 </since_tizen>
    public class VisualView : CustomView
    {
        private Dictionary<int, string> visualNameDictionary;
        private Dictionary<int, VisualBase> visualDictionary;
        private Dictionary<int, PropertyMap> tranformDictionary;
        private PropertyArray animateArray;

        /// <summary>
        /// Constructor.
        /// This constructor initializes the VisualView with default behavior and support for touch events.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public VisualView() : this(CustomViewBehaviour.ViewBehaviourDefault | CustomViewBehaviour.RequiresTouchEventsSupport)
        {
        }

        /// This will be public opened after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public VisualView(ViewStyle viewStyle) : this(CustomViewBehaviour.ViewBehaviourDefault | CustomViewBehaviour.RequiresTouchEventsSupport, viewStyle)
        {

        }

        /// <summary>Create a VisualView with specified behaviour.</summary>
        /// <param name="behaviour">CustomView behaviour</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public VisualView(CustomViewBehaviour behaviour) : base(typeof(VisualView).FullName, behaviour)
        {
        }

        /// <summary>Create a VisualView with specified behaviour and a style.</summary>
        /// <param name="behaviour">CustomView behaviour</param>
        /// <param name="viewStyle">The ViewStyle.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public VisualView(CustomViewBehaviour behaviour, ViewStyle viewStyle) : base(typeof(VisualView).FullName, behaviour, viewStyle)
        {
        }

        // static constructor registers the control type (for user can add kinds of visuals to it)
        static VisualView()
        {
            // ViewRegistry registers control type with DALi type registry
            // also uses introspection to find any properties that need to be registered with type registry
            CustomViewRegistry.Instance.Register(CreateInstance, typeof(VisualView));
        }

        /// <summary>
        /// Gets the total number of visuals which are added by users.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public int NumberOfVisuals
        {
            get
            {
                return visualDictionary.Count;
            }
        }

        /// <summary>
        /// Overrides the parent method.
        /// This method is called by the framework when the instance is created.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public override void OnInitialize()
        {
            base.OnInitialize();

            //Initialize empty
            visualNameDictionary = new Dictionary<int, string>();
            visualDictionary = new Dictionary<int, VisualBase>();
            tranformDictionary = new Dictionary<int, PropertyMap>();
            animateArray = new PropertyArray();
        }

        /// <summary>
        /// Adds or updates a visual to visual view.
        /// </summary>
        /// <param name="visualName">The name of a visual to add. If a name is added to an existing visual name, the visual will be replaced.</param>
        /// <param name="visualMap">The property map of a visual to create.</param>
        /// <exception cref="ArgumentNullException"> Thrown when visualMap is null. </exception>
        /// <since_tizen> 3 </since_tizen>
        public void AddVisual(string visualName, VisualMap visualMap)
        {
            int visualIndex = -1;

            /* If the visual had added, then replace it using RegisterVusal. */
            //visual.Name = name;
            foreach (var item in visualNameDictionary)
            {
                if (item.Value == visualName)
                {
                    /* Find a existed visual, its key also exited. */
                    visualIndex = item.Key;
                    UnregisterVisual(visualIndex);
                    visualNameDictionary.Remove(visualIndex);
                    visualDictionary.Remove(visualIndex);
                    tranformDictionary.Remove(visualIndex);
                    break;
                }
            }

            if (visualIndex == -1) // The visual is a new one, create index for it. */
            {
                using (var temp = new PropertyValue(visualName))
                {
                    visualIndex = RegisterProperty(visualName, temp, PropertyAccessMode.ReadWrite);
                }
            }

            if (visualIndex > 0)
            {
                if (visualMap == null)
                {
                    throw new ArgumentNullException(nameof(visualMap));
                }

                visualMap.VisualIndex = visualIndex;
                visualMap.Name = visualName;
                visualMap.Parent = this;

                // Register index and name
                UpdateVisual(visualIndex, visualName, visualMap);
            }
        }

        /// <summary>
        /// Removes a visual by name.
        /// </summary>
        /// <param name="visualName">The name of a visual to remove.</param>
        /// <since_tizen> 3 </since_tizen>
        public void RemoveVisual(string visualName)
        {
            foreach (var item in visualNameDictionary)
            {
                if (item.Value == visualName)
                {
                    EnableVisual(item.Key, false);
                    UnregisterVisual(item.Key);
                    tranformDictionary.Remove(item.Key);
                    visualDictionary.Remove(item.Key);
                    visualNameDictionary.Remove(item.Key);

                    RelayoutRequest();
                    break;
                }
            }
        }

        /// <summary>
        /// This method removes all visuals associated with the VisualView instance.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public void RemoveAll()
        {
            foreach (var item in visualNameDictionary)
            {
                EnableVisual(item.Key, false);
                UnregisterVisual(item.Key);
            }
            visualDictionary.Clear();
            tranformDictionary.Clear();
            visualNameDictionary.Clear();
            RelayoutRequest();
        }

        /// <summary>
        /// Overrides the method of OnRelayout() for CustomView class.<br />
        /// Called after the size negotiation has been finished for this control.<br />
        /// The control is expected to assign this given size to itself or its children.<br />
        /// Should be overridden by derived classes if they need to layout actors differently after certain operations like add or remove actors, resize, or after changing specific properties.<br />
        /// </summary>
        /// <remarks>As this function is called from inside the size negotiation algorithm, you cannot call RequestRelayout (the call would just be ignored).</remarks>
        /// <param name="size">The allocated size.</param>
        /// <param name="container">The control should add actors to this container that it is not able to allocate a size for.</param>
        /// <since_tizen> 3 </since_tizen>
        public override void OnRelayout(Vector2 size, RelayoutContainer container)
        {
            foreach (var item in visualDictionary)
            {
                if(item.Value != null)
                {
                    item.Value.SetTransformAndSize(tranformDictionary[item.Key], size);
                    EnableVisual(item.Key, true);
                }
            }
        }

        /// <summary>
        /// Creates a visual animation (transition) with the input parameters.
        /// </summary>
        /// <param name="target">The visual map to animation.</param>
        /// <param name="property">The property of visual to animation.</param>
        /// <param name="destinationValue">The destination value of property after animation.</param>
        /// <param name="startTime">The start time of visual animation.</param>
        /// <param name="endTime">The end time of visual animation.</param>
        /// <param name="alphaFunction">The alpha function of visual animation.</param>
        /// <param name="initialValue">The initial property value of visual animation.</param>
        /// <returns>Animation instance</returns>
        /// <exception cref="ArgumentNullException"> Thrown when target is null. </exception>
        /// <since_tizen> 3 </since_tizen>
        public Animation AnimateVisual(VisualMap target, string property, object destinationValue, int startTime, int endTime, AlphaFunction.BuiltinFunctions? alphaFunction = null, object initialValue = null)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            string strAlpha = alphaFunction?.GetDescription();

            foreach (var item in visualNameDictionary.ToList())
            {
                if (item.Value == target.Name)
                {
                    using (PropertyMap animator = new PropertyMap())
                    using (PropertyMap timePeriod = new PropertyMap())
                    using (PropertyValue destVal = PropertyValue.CreateFromObject(destinationValue))
                    using (PropertyMap transition = new PropertyMap())
                    {
                        if (strAlpha != null)
                        {
                            animator.Add("alphaFunction", strAlpha);
                        }
                        timePeriod.Add("duration", (endTime - startTime) / 1000.0f);
                        timePeriod.Add("delay", startTime / 1000.0f);
                        animator.Add("timePeriod", timePeriod);

                        StringBuilder sb = new StringBuilder(property);
                        sb[0] = (char)(sb[0] | 0x20);
                        string _str = sb.ToString();
                        if (_str == "position") { _str = "offset"; }

                        transition.Add("target", target.Name);
                        transition.Add("property", _str);

                        if (initialValue != null)
                        {
                            using (PropertyValue initVal = PropertyValue.CreateFromObject(initialValue))
                            using (PropertyValue pvInitialValue = new PropertyValue(initVal))
                            {
                                transition.Add("initialValue", pvInitialValue);
                            }
                        }
                        transition.Add("targetValue", destVal);
                        transition.Add("animator", animator);

                        using (TransitionData transitionData = new TransitionData(transition))
                        {
                            return this.CreateTransition(transitionData);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Adds a group visual animation (transition) map with the input parameters.
        /// </summary>
        /// <param name="target">The visual map to animation.</param>
        /// <param name="property">The property of visual to animation.</param>
        /// <param name="destinationValue">The destination value of property after animation.</param>
        /// <param name="startTime">The start time of visual animation.</param>
        /// <param name="endTime">The end time of visual animation.</param>
        /// <param name="alphaFunction">The alpha function of visual animation.</param>
        /// <param name="initialValue">The initial property value of visual animation.</param>
        /// <exception cref="ArgumentNullException"> Thrown when target is null. </exception>
        /// <since_tizen> 3 </since_tizen>
        public void AnimateVisualAdd(VisualMap target, string property, object destinationValue, int startTime, int endTime, AlphaFunction.BuiltinFunctions? alphaFunction = null, object initialValue = null)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            string strAlpha = alphaFunction?.GetDescription();

            foreach (var item in visualNameDictionary.ToList())
            {
                if (item.Value == target.Name)
                {
                    using (PropertyMap animator = new PropertyMap())
                    using (PropertyMap timePeriod = new PropertyMap())
                    using (PropertyValue destVal = PropertyValue.CreateFromObject(destinationValue))
                    using (PropertyMap transition = new PropertyMap())
                    {
                        if (strAlpha != null)
                        {
                            animator.Add("alphaFunction", strAlpha);
                        }

                        timePeriod.Add("duration", (endTime - startTime) / 1000.0f);
                        timePeriod.Add("delay", startTime / 1000.0f);
                        animator.Add("timePeriod", timePeriod);

                        StringBuilder sb = new StringBuilder(property);
                        sb[0] = (char)(sb[0] | 0x20);
                        string _str = sb.ToString();
                        if (_str == "position") { _str = "offset"; }

                        transition.Add("target", target.Name);
                        transition.Add("property", _str);

                        if (initialValue != null)
                        {
                            using (PropertyValue initVal = PropertyValue.CreateFromObject(initialValue))
                            using (PropertyValue pvInitialValue = new PropertyValue(initVal))
                            {
                                transition.Add("initialValue", pvInitialValue);
                            }
                        }
                        transition.Add("targetValue", destVal);
                        transition.Add("animator", animator);

                        using (PropertyValue pvTransition = new PropertyValue(transition))
                        {
                            PropertyArray temp = animateArray.Add(pvTransition);
                            temp.Dispose();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finishes to add a visual animation (transition) map and creates a transition animation.
        /// </summary>
        /// <returns>Animation instance.</returns>
        /// <since_tizen> 3 </since_tizen>
        public Animation AnimateVisualAddFinish()
        {
            if (animateArray == null || animateArray.Empty())
            {
                Tizen.Log.Fatal("NUI", "animate visual property array is empty!");
                return null;
            }
            TransitionData transitionData = new TransitionData(animateArray);
            Animation ret = this.CreateTransition(transitionData);
            transitionData.Dispose();
            return ret;
        }

        /// <summary>
        /// Applies an animation to the specified visual map properties.
        /// </summary>
        /// <exception cref="ArgumentNullException"> Thrown when visualMap is null. </exception>
        /// <since_tizen> 3 </since_tizen>
        public Animation VisualAnimate(Tizen.NUI.VisualAnimator visualMap)
        {
            if (visualMap == null)
            {
                throw new ArgumentNullException(nameof(visualMap));
            }

            foreach (var item in visualNameDictionary.ToList())
            {
                if (item.Value == visualMap.Target)
                {
                    using (TransitionData transitionData = new TransitionData(visualMap.OutputVisualMap))
                    {
                        return this.CreateTransition(transitionData);
                    }
                }
            }
            return null;
        }
        //temporary fix to pass TCT

        internal void UpdateVisual(int visualIndex, string visualName, VisualMap visualMap)
        {
            VisualBase visual = VisualFactory.Instance.CreateVisual(visualMap.OutputVisualMap);

            visualNameDictionary[visualIndex] = visualName;
            visualDictionary[visualIndex] = visual;
            tranformDictionary[visualIndex] = visualMap.OutputTransformMap;

            if(visual != null)
            {
                visual.Name = visualName;
                visual.DepthIndex = visualMap.DepthIndex;

                RegisterVisual(visualIndex, visual);

                RelayoutRequest();
                NUILog.Debug("UpdateVisual() name=" + visualName);
            }
            else
            {
                NUILog.Debug("UpdateVisual() FAIL! visual create failed name=" + visualName);
            }
        }

        static CustomView CreateInstance()
        {
            return new VisualView();
        }

    }
}
