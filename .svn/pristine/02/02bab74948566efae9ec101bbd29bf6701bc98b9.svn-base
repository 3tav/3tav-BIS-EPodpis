using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mSignLib
{
    public class GetDocumentsDTO
    {
        public int id { get; set; }                         // Document's ID
        public string title { get; set; }                   // Document's Title
        public DateTime createdAt { get; set; }             // Document's Creation date
        public string uuid { get; set; }                    // Document's Universal Unique Identifier
        public int status { get; set; }                     // 0 - draft, 1 - for signing, 2 - signed
    }

    public class GetDocumentDTO
    {
        public int id { get; set; }                       // Document's ID
        public string title { get; set; }                   // Title
        public DateTime createdAt { get; set; }             // Creation date
        public int createdById { get; set; }                // Creation User ID
        public string uuid { get; set; }	                // Universal Unique Identifier
        public int status { get; set; }                     // 0 - draft, 1 - for signing, 2 - signed
        public List<Attachment> Attachments { get; set; }
    }

    public class Attachment
    {
        public int id { get; set; }                         // Attachment ID
        public string fileName { get; set; }                // Attachment File Name
        public string lng { get; set; }                     // Attachment Language
        public string uuid { get; set; }                    // Attachment Universal Unique Identifier
        public string content { get; set; }                 // Base64 Attachment File
        public List<SignatureTag> signatureTags { get; set; }
    }
    public class SignatureTag
    {
        public string tag { get; set; }
        public string metadataValue { get; set; }
    }
    public class Signature
    {
        public int id { get; set; }                         // Signature ID
        public int width { get; set; }                      // Signature Width
        public int height { get; set; }                     // Signature Height
        public int btag { get; set; }                       // Signature template Tag
        public int x { get; set; }                          // Signature x axis position
        public int y { get; set; }                          // Signature y axis position
        public int page { get; set; }                       // Document page number
        public int signed { get; set; }                     // 0 - unsigned, 1 - Signed
        public int templateId { get; set; }                 // Template ID
        public string description { get; set; }             // Signature Description translations in JSON
        public string type { get; set; }                    // Signature Type
        public object metadataParams { get; set; }          // Signature metadata parameters
        public string metadataValue { get; set; }           // Signature metadata values
    }

    public class CreateDocumentDTO
    {
        public string title { get; set; }                   // Title
        public string username { get; set; }                // Username
        public string userId { get; set; }                  // UserID
        public string description { get; set; }             // document Description
        public int status { get; set; }                     // 0 - draft, 1 - for signing, 2 - signed
        public List<Attachment> attachments { get; set; }
    }


    public class CreateSharedDocumentDTO
    {
        public string signatureTagName { get; set; }        // Signature document tag name
        public int documentId { get; set; }
        public string username { get; set; }                // Username
        public string userId { get; set; }                  // UserID
        public string email { get; set; }                   // share document for signing to email
        public string mobile_number { get; set; }           // mobile number for password delivery
        public string lng { get; set; }                     // Language
        public string signatureType { get; set; }           // Signature type override
        public string description { get; set; }             // Description for signer        
    }

    public class CreateSharedDocumentReturnDTO
    {
        public int Id { get; set; }
        public int documentId { get; set; }
        public string userId { get; set; }                  // UserID
        public string email { get; set; }                   // share document for signing to email
        public string lng { get; set; }                     // Language
        public int signatureTemplateId { get; set; }           // 
        public string token { get; set; }
        public DateTime createdAt { get; set; }
        public int createdById { get; set; }
        public DateTime expiresOn { get; set; }
    }

    public enum mSignDocumentStatus
    {
        Draft = 0,
        ForSigning = 1,
        Signed = 2
    }

}
