using HangMan.Model.Model;

namespace HangMan.Service.Services
{
    public class WordService : ServiceBase<Word>
    {
        public WordService(string securityToken) : this("api/Words", securityToken) { }

        private WordService(string baseAddress, string securityToken) : base(baseAddress, securityToken) { }
    }
}
