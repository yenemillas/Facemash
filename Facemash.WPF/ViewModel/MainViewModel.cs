using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using Facemash.WPF.DTO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System.Linq;

namespace Facemash.WPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        #region Fields

        private string _messageButton;

        private bool _isVoteMode;

        private int _vote;

        private string _cat_1;

        private string _cat_2;

        private List<Cat> _cats;

        #endregion

        #region Properties

        public string MessageButton
        {
            get { return _messageButton; }
            set { _messageButton = value; RaisePropertyChanged("MessageButton"); }
        }

        public bool IsVoteMode
        {
            get { return _isVoteMode; }
            set { _isVoteMode = value; RaisePropertyChanged("IsVoteMode"); }
        }

        public string User { get; set; }

        public int Vote
        {
            get { return _vote; }
            set { _vote = value; RaisePropertyChanged("Vote"); }
        }

        public string Cat_1
        {
            get { return _cat_1; }
            set
            {
                _cat_1 = value;
                RaisePropertyChanged("Cat_1");
            }
        }

        public string Cat_2
        {
            get { return _cat_2; }
            set
            {
                _cat_2 = value;
                RaisePropertyChanged("Cat_2");
            }
        }

        public Match CurrentMatch { get; set; }

        public List<Cat> Cats
        {
            get { return _cats; }
            set { _cats = value; RaisePropertyChanged("Cats"); }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            GetMatch();
            InitializeCommand();
            Vote = 0;
            IsVoteMode = true;
            MessageButton = "Voir les plus beaux chats";
            User = Environment.UserName;
        } 
        #endregion

        #region Commands

        public RelayCommand<string> PostMatchCommand { get; private set; }

        public RelayCommand GetResultCommand { get; private set; }

        private void InitializeCommand()
        {
            PostMatchCommand = new RelayCommand<string>(PostMatch);
            GetResultCommand = new RelayCommand(GetResult);
        }

        #endregion

        #region Methodes

        /// <summary>
        /// Post Match
        /// </summary>
        /// <param name="result"></param>
        private async void PostMatch(string result)
        {
            CurrentMatch.Result = result == "1";
            var httpContent = GetHttpContent(CurrentMatch);
            using (HttpClient client = new HttpClient())
            {
                await client.PostAsync(@"https://localhost:44393/api/Cats/PostMatch", httpContent);
            }

            GetMatch();
            Vote++;
        }

        /// <summary>
        /// Get Match
        /// </summary>
        private async void GetMatch()
        {
            using (HttpClient client = new HttpClient())
            {
                var clientReponse = await client.GetAsync(@"https://localhost:44393/api/Cats/GetMatch").ContinueWith(x => x.Result.Content.ReadAsStringAsync());
                CurrentMatch = JsonConvert.DeserializeObject<Match>(clientReponse.Result);
                Cat_1 = CurrentMatch.Cat_1.Url;
                Cat_2 = CurrentMatch.Cat_2.Url;
            }
        }

        /// <summary>
        /// Get Http Content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private HttpContent GetHttpContent<T>(T obj)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            var jsonValue = JsonConvert.SerializeObject(obj, jsonSerializerSettings);
            return new StringContent(jsonValue, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Get Result
        /// </summary>
        private async void GetResult()
        {
            IsVoteMode = !IsVoteMode;

            if (!IsVoteMode)
            {
                using (HttpClient client = new HttpClient())
                {
                    var clientReponse = await client.GetAsync(@"https://localhost:44393/api/Cats/GetResult").ContinueWith(x => x.Result.Content.ReadAsStringAsync());
                    var result = JsonConvert.DeserializeObject<List<Cat>>(clientReponse.Result);
                    Cats = result.OrderByDescending(x => x.Ranking).ToList();
                }
                MessageButton = "Voter pour les chats";
            }
            else
            {
                MessageButton = "Voir les plus beaux chats";
            }
        } 

        #endregion
    }
}