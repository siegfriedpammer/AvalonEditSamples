// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.ComponentModel.Design;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.AddIn;
using ICSharpCode.SharpDevelop.Editor;

namespace TextMarkerSample
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		public Window1()
		{
			InitializeComponent();
			InitializeTextMarkerService();
		}

		ITextMarkerService textMarkerService;
		
		void InitializeTextMarkerService()
		{
			var textMarkerService = new TextMarkerService(textEditor.Document);
			textEditor.TextArea.TextView.BackgroundRenderers.Add(textMarkerService);
			textEditor.TextArea.TextView.LineTransformers.Add(textMarkerService);
			IServiceContainer services = (IServiceContainer)textEditor.Document.ServiceProvider.GetService(typeof(IServiceContainer));
			if (services != null)
				services.AddService(typeof(ITextMarkerService), textMarkerService);
			this.textMarkerService = textMarkerService;
		}
		
		void RemoveAllClick(object sender, RoutedEventArgs e)
		{
			textMarkerService.RemoveAll(m => true);
		}
		
		void RemoveSelectedClick(object sender, RoutedEventArgs e)
		{
			textMarkerService.RemoveAll(IsSelected);
		}
		
		void AddMarkerFromSelectionClick(object sender, RoutedEventArgs e)
		{
			ITextMarker marker = textMarkerService.Create(textEditor.SelectionStart, textEditor.SelectionLength);
			marker.MarkerTypes = TextMarkerTypes.SquigglyUnderline;
			marker.MarkerColor = Colors.Red;
		}
		
		bool IsSelected(ITextMarker marker)
		{
			int selectionEndOffset = textEditor.SelectionStart + textEditor.SelectionLength;
			if (marker.StartOffset >= textEditor.SelectionStart && marker.StartOffset <= selectionEndOffset)
				return true;
			if (marker.EndOffset >= textEditor.SelectionStart && marker.EndOffset <= selectionEndOffset)
				return true;
			return false;
		}
	}
}