﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本: 16.0.0.0
//  
//     对此文件的更改可能导致不正确的行为，如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ElasticSearch.Template.Java
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using ElasticSearch.Loader.Model;
    using Infrastructure;
    using Savory;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class EsFieldsTemplate : EsFieldsTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("package ");
            
            #line 9 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.JavaRoot));
            
            #line default
            #line hidden
            this.Write(@";

/*
 *------------------------------------------------------------------------------
 *     DO NOT GO GENTLE INTO THAT GOOD NIGHT.
 *
 *     harriszhang@live.cn
 *------------------------------------------------------------------------------
 */

");
            
            #line 19 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"

        if(!string.IsNullOrEmpty(this.ClassNode.Summary))
        {

            
            #line default
            #line hidden
            this.Write("/**\r\n * ");
            
            #line 24 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.ClassNode.Summary));
            
            #line default
            #line hidden
            this.Write("\r\n */\r\n");
            
            #line 26 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"

        }

            
            #line default
            #line hidden
            this.Write("public final class ");
            
            #line 29 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.ClassNode.Name));
            
            #line default
            #line hidden
            this.Write("Fields {\r\n");
            
            #line 30 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"

        if (this.ClassNode.PropertyNodeList != null && this.ClassNode.PropertyNodeList.Count > 0)
        {
            foreach (PropertyNode propertyNode in this.ClassNode.PropertyNodeList)
            {
                if(!string.IsNullOrEmpty(propertyNode.Summary))
                {

            
            #line default
            #line hidden
            this.Write("\r\n    /**\r\n     * ");
            
            #line 40 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(propertyNode.Summary));
            
            #line default
            #line hidden
            this.Write("\r\n     */\r\n");
            
            #line 42 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"

                }

            
            #line default
            #line hidden
            this.Write("    public final static String ");
            
            #line 45 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(propertyNode.Name.ToUpperCaseUnderLine()));
            
            #line default
            #line hidden
            this.Write(" = \"");
            
            #line 45 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(propertyNode.Name.ToLowerCaseUnderLine()));
            
            #line default
            #line hidden
            this.Write("\";\r\n");
            
            #line 46 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"

                if(propertyNode.FieldAttribute == null)
                {
                    continue;
                }

                if(propertyNode.FieldAttribute is TextFieldAttribute)
                {
                    var textFieldAttribute = propertyNode.FieldAttribute as TextFieldAttribute;
                    if(!string.IsNullOrEmpty(propertyNode.Summary))
                    {

            
            #line default
            #line hidden
            this.Write("\r\n    /**\r\n     * ");
            
            #line 60 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(propertyNode.Summary));
            
            #line default
            #line hidden
            this.Write("(使用`keyword`，不分词)\r\n     */\r\n");
            
            #line 62 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"

                    }

            
            #line default
            #line hidden
            this.Write("    public final static String ");
            
            #line 65 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture($"{propertyNode.Name.ToUpperCaseUnderLine()}_KEYWORD"));
            
            #line default
            #line hidden
            this.Write(" = \"");
            
            #line 65 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(propertyNode.Name.ToLowerCaseUnderLine()));
            
            #line default
            #line hidden
            this.Write(".keyword\";\r\n");
            
            #line 66 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"

                    //分词器
                    List<string> analyzers = new List<string>();

                    //内置分词器
                    var builtInAnalyzers = textFieldAttribute.BuiltInAnalyzer.ToString().ToLower().Split(CommaAndWhitespace, StringSplitOptions.RemoveEmptyEntries).OrderBy(x => x).ToList();
                    analyzers.AddRange(builtInAnalyzers);

                    //ik分词器
                    var ikAnalyzers = textFieldAttribute.IKAnalyzer.ToString().ToLower().Split(CommaAndWhitespace, StringSplitOptions.RemoveEmptyEntries).OrderBy(x => x).ToList();
                    analyzers.AddRange(ikAnalyzers);

                    //自定义分词器
                    if (textFieldAttribute.CustomAnalyzer != null && textFieldAttribute.CustomAnalyzer.Length > 0)
                    {
                        analyzers.AddRange(textFieldAttribute.CustomAnalyzer);
                    }

                    foreach (var analyzer in analyzers)
                    {
                        if ("none".Equals(analyzer, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        if(!string.IsNullOrEmpty(propertyNode.Summary))
                        {

            
            #line default
            #line hidden
            this.Write("\r\n    /**\r\n     * ");
            
            #line 96 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(propertyNode.Summary));
            
            #line default
            #line hidden
            this.Write("(使用`");
            
            #line 96 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(analyzer));
            
            #line default
            #line hidden
            this.Write("`分词器)\r\n     */\r\n");
            
            #line 98 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"

                        }

            
            #line default
            #line hidden
            this.Write("    public final static String ");
            
            #line 101 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture($"{propertyNode.Name.ToUpperCaseUnderLine()}_{analyzer.ToUpperCaseUnderLine()}"));
            
            #line default
            #line hidden
            this.Write(" = \"");
            
            #line 101 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture($"{propertyNode.Name.ToLowerCaseUnderLine()}.{analyzer}"));
            
            #line default
            #line hidden
            this.Write("\";\r\n");
            
            #line 102 "D:\TheGitlabWorkspace\harris-app\elasticsearch\ElasticSearch\Template\Java\EsFieldsTemplate.tt"

                    }
                }
            }
        }

            
            #line default
            #line hidden
            this.Write("}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class EsFieldsTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
