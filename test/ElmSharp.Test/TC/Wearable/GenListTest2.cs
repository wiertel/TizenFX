/*
 * Copyright (c) 2016 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using ElmSharp;
using System.Collections.Generic;

namespace ElmSharp.Test.Wearable
{
    class GenListTest2 : WearableTestCase
    {
        public override string TestName => "GenListTest2";
        public override string TestDescription => "To test ScrollTo operation of GenList";

        public override void Run(Window window)
        {
            Conformant conformant = new Conformant(window);
            conformant.Show();

            GenList list = new GenList(window)
            {
                Homogeneous = true,
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1
            };

            GenItemClass defaultClass = new GenItemClass("default")
            {
                GetTextHandler = (obj, part) =>
                {
                    return string.Format("{0} - {1}", (string)obj, part);
                }
            };

            GenListItem[] items = new GenListItem[100];
            int i = 0;
            for (i = 0; i < 100; i++)
            {
                items[i] = list.Append(defaultClass, string.Format("{0} Item", i));
            }
            list.Show();
            list.ItemSelected += List_ItemSelected;

            GenListItem scroll = items[0];

            Button first = new Button(window)
            {
                Text = "F",
                AlignmentX = -1,
                WeightX = 1,
            };
            Button last = new Button(window)
            {
                Text = "L",
                AlignmentX = -1,
                WeightX = 1,
            };
            Button add = new Button(window)
            {
                Text = "A",
                AlignmentX = -1,
                WeightX = 1,
            };
            add.Clicked += (s, e) =>
            {
                scroll = list.InsertBefore(defaultClass, string.Format("{0} Item", i++), scroll);
                list.ScrollTo(scroll, ScrollToPosition.In, false);
            };
            first.Clicked += (s, e) =>
            {
                list.ScrollTo(scroll, ScrollToPosition.In, true);
            };
            last.Clicked += (s, e) =>
            {
                list.ScrollTo(items[99], ScrollToPosition.In, true);
            };
            first.Show();
            last.Show();
            add.Show();

            var square = window.GetInnerSquare();

            list.Geometry = new Rect(square.X, square.Y, square.Width, square.Height * 3 / 4);
            first.Geometry = new Rect(square.X, square.Y + square.Height * 3 / 4, square.Width / 3, square.Height / 4);
            last.Geometry = new Rect(square.X + square.Width / 3, square.Y + square.Height * 3 / 4, square.Width / 3, square.Height / 4);
            add.Geometry = new Rect(square.X + square.Width * 2 / 3, square.Y + square.Height * 3 / 4, square.Width / 3, square.Height / 4);
        }

        private void List_ItemSelected(object sender, GenListItemEventArgs e)
        {
            Console.WriteLine("{0} Item was selected", (string)(e.Item.Data));
        }
    }
}
