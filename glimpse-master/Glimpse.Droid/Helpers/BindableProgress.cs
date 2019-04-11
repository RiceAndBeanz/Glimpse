using Android.App;
using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Droid.Helpers
{
    public class BindableProgress
    {
        private ProgressDialog _dialog;
        private Context _context;
        private string _title;

        public BindableProgress(Context context)
        {
            _context = context;
            _title = "";
        }

        public bool Visible
        {
            get {return _dialog != null; }
            set
            {
                if (value == Visible)
                    return;
                if (value)
                {
                    _dialog = new ProgressDialog(_context);
                    _dialog.SetMessage(Title);
                    _dialog.Show();
                }
                else
                {
                    _dialog.Hide();
                    _dialog = null;
                }
            }
        }

       
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
            }
        }


    }
}
