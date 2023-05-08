using System.Text.RegularExpressions;

namespace PiWord.Models
{
    public class Dictionary
    {
        public List<Word> Words { get; set; }

        public Dictionary()
        {
            this.Words = RetrieveWords();
        }

        internal Word Random()
        {
            var random = new Random();
            var index = random.Next(this.Words.Count);
            return this.Words[index];
        }

        private List<Word> RetrieveWords()
        {
            const string fileName = "dictionary.json";
            using var stream = FileSystem.OpenAppPackageFileAsync(fileName);
            var words = new List<Word>();

            using (var reader = new StreamReader(stream.Result))
            {
                var content = reader.ReadToEnd();
                var regex = new Regex(@"^[^a-zA-Z0-9]*|[^a-zA-Z0-9]*$");

                var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var parts = line.Split(new[] { " : " }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2) continue;
                    var value = regex.Replace(parts[0], "");
                    var definition = parts[1].Trim('"');
                    var word = new Word { Value = value, Definition = definition };
                    words.Add(word);
                }
            }
            return words;
        }

        public static async Task<string> ReadTextFile(string filePath)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(filePath);
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

    }
}
