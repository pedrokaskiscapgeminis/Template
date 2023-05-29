/*#nullable enable
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using Mochineko.Whisper_API;
using Mochineko.Whisper_API.Transcription;

namespace XXX
{
    /// <summary>
    /// A sample component to transcribe speech by Whisper API on Unity.https://github.com/mochi-neko/Whisper-API-unity
    /// </summary>
    public sealed class TranscriptionSample : MonoBehaviour
    {
        /// <summary>
        /// API key generated by OpenAPI.
        /// </summary>
        [SerializeField] private string apiKey = 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;

        /// <summary>
        /// File path of speech audio.
        /// </summary>
        [SerializeField] private string filePath = "C:/Users/mcocerap/Downloads";

        private WhisperTranscriptionConnection? connection;

        private void Start()
        {
            // Create instance of WhisperTranscriptionConnection.
            connection = new WhisperTranscriptionConnection(apiKey);

            // If you want to specify response format, language, etc..., please use other initialization:
            // connection = new WhisperTranscriptionConnection(apiKey, new APIRequestBody(
            //     file: "",
            //     model: "whisper-1",
            //     prompt: "Some prompts",
            //     responseFormat: "json",
            //     temperature: 1f,
            //     language: "ja"));
        }

        [ContextMenu(nameof(Transcribe))]
        public async void Transcribe()
        {
            string result;
            try
            {
                // Transcribe speech by Whisper speech to text API.
                result = await connection
                    .TranscribeFromFileAsync(filePath, this.GetCancellationTokenOnDestroy());
            }
            catch (Exception e)
            {
                // Exceptions should be caught.
                Debug.LogException(e);
                return;
            }

            // Default text response format is JSON.
            var text = APIResponseBody.FromJson(result)?.Text;

            // Log text result.
            Debug.Log($"[Whisper_API.Transcription.Samples] Result:\n{text}");

        }
    }
}*/
