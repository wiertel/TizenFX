/*
 * Copyright(c) 2021 Samsung Electronics Co., Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System;
using System.ComponentModel;
using Tizen.NUI.Binding;

namespace Tizen.NUI.Xaml
{
    /// <summary>
    /// Provide application's shared resource path for xaml.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ContentProperty(nameof(FilePath))]
    [AcceptEmptyServiceProvider]
    public class SharedResourcePathExtension : IMarkupExtension<string>
    {
        /// <summary>
        /// Provide application's shared resource path for xaml.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SharedResourcePathExtension()
        {
        }

        /// <summary> User should specify file path as a content of this extension. </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string FilePath { get; set; }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string ProvideValue(IServiceProvider serviceProvider) => Tizen.Applications.Application.Current.DirectoryInfo.SharedResource + FilePath;

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return (this as IMarkupExtension<string>).ProvideValue(serviceProvider);
        }
    }
}
