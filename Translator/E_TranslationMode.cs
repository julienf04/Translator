using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translations
{
    /// <summary>
    /// Defines what will be translated
    /// </summary>
    public enum E_TranslationMode
    {
        /// <summary>
        /// Translate string
        /// </summary>
        translate,
        /// <summary>
        /// Translate documents. Available documents are: .doc, .docx, .odf, .pdf, .ppt, .pptx, .ps, .rtf, .txt, .xls o .xlsx
        /// </summary>
        docs
    }
}
