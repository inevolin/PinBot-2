using System;
using PinBot2.Presenter;
using PinBot2.Dal;
using PinBot2.Presenter.Configurations;
using PinBot2.Configurations.PinForms;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace PinBot2
{ 
    static class Program
    {
        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Program.IsAlreadyRunning())
            {
                MessageBox.Show("PinBot is already running.");
                return;
            }

            bool PREMIUM = false;
            var splash = new SplashPresenter(new SplashForm());
            splash.ShowForm();

            if (!splash.MayContinue)
                return;

            if (!splash.IsValid)
            {
                var licensing = new LicensingPresenter(new LicensingForm());
                licensing.ShowForm();

                if (!licensing.MayContinue)
                    return;

                PREMIUM = licensing.IS_PREMIUM;

                if (!PREMIUM && !splash.IsTrialMember)
                {
                    var trial = new TrialPresenter(new TrialForm());
                    trial.ShowForm();
                    if (!trial.MayContinue)
                        return;
                }
            }
            else
            {
                PREMIUM = true;
            }

            Console.WriteLine("PREMIUM:  " + PREMIUM);

            var accountForm = new AccountForm(PREMIUM);
            var InMemoryAccountRepository = new InMemoryAccountRepository();

            var  addEditAccountForm = new AddEditAccountForm();
            var selectForm = new SelectForm();
            ///////
            var likeForm = new LikeForm(PREMIUM);
            var followForm = new FollowForm(PREMIUM);
            var unfollowForm = new UnfollowForm(PREMIUM);
            var repinForm = new RepinForm(PREMIUM);
            var inviteForm = new InviteForm(PREMIUM);
            var PinForm = new PinForm(PREMIUM);
            var commentForm = new CommentForm(PREMIUM);
            var pinQueueForm = new QueueForm();
            var repinQueueForm = new QueueForm();
            //....
            var configPresenter = new ConfigPresenter(InMemoryAccountRepository, selectForm, likeForm,followForm, unfollowForm, repinForm,inviteForm, PinForm, pinQueueForm, repinQueueForm, commentForm, PREMIUM);
            var addEditAccountPresenter = new AddEditAccountPresenter(InMemoryAccountRepository, addEditAccountForm,PREMIUM);
            var accountPresenter = new AccountPresenter(accountForm,
                                                        InMemoryAccountRepository,
                                                        addEditAccountPresenter,
                                                        configPresenter,
                                                        PREMIUM
                                                        );

            accountPresenter.ShowForm();
        }

        public static bool IsAlreadyRunning()
        {
            Process mobj_pro = Process.GetCurrentProcess();
            Process[] mobj_proList = Process.GetProcessesByName(mobj_pro.ProcessName);
            if (mobj_proList.Length > 1)
            {
                return true;
            }
            return false;
        }
    }
}