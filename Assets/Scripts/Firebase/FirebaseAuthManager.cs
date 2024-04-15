using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.SceneManagement;

public class FirebaseAuthManager : MonoBehaviour
{
    public static FirebaseAuthManager Instance;

    [Header("Firebase base")]
    public DependencyStatus DependencyStatus;
    public FirebaseAuth Auth;
    public FirebaseUser User;

    [Space]
    [Header("Login")]
    [SerializeField] private TMP_InputField EmailLoginInputField;
    [SerializeField] private TMP_InputField PasswordLoginInputField;
    [SerializeField] private Button LoginButton;
    [SerializeField] private Button LogoutButton;

    [Space]
    [Header("Register")]
    [SerializeField] private TMP_InputField NameRegisterInputField;
    [SerializeField] private TMP_InputField EmailRegisterInputField;
    [SerializeField] private TMP_InputField PasswordRegisterInputField;
    [SerializeField] private TMP_InputField ConfirmPasswordRegisterInputField;
    [SerializeField] private Button RegisterButton;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        StartCoroutine(CheckAndFixDependenciesAsync());
    }

    private IEnumerator CheckAndFixDependenciesAsync()
    {
        Task<DependencyStatus> dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        DependencyStatus = dependencyTask.Result;

        if (DependencyStatus == DependencyStatus.Available)
        {
            IntiliazeFirebase();
            yield return new WaitForEndOfFrame();
            StartCoroutine(CheckForAutoLogin());
        }
        else
        {
            Debug.LogError("CheckAndFixDependenciesAsyncError");
        }
    }

    private void OnEnable()
    {
        LoginButton.onClick.AddListener(Login);
        if (LogoutButton != null) { LoginButton.onClick.AddListener(LogOut); }
        RegisterButton.onClick.AddListener(Register);
    }

    private void OnDisable()
    {
        if (LoginButton != null) { LoginButton.onClick.RemoveListener(Login); }
        if (LogoutButton != null) { LoginButton.onClick.RemoveListener(LogOut); }
        if (RegisterButton != null) { LoginButton.onClick.RemoveListener(Register); }
    }

    private void IntiliazeFirebase()
    {
        Auth = FirebaseAuth.DefaultInstance;

        Auth.StateChanged += Auth_StateChanged;
        Auth_StateChanged(this, null);
    }

    private IEnumerator CheckForAutoLogin()
    {
        if (User != null)
        {
            Task reloadUser = User.ReloadAsync();

            yield return new WaitUntil(() => reloadUser.IsCompleted);

            AutoLogin();
        }
        else
        {
            UIManager.Instance.OpenLoginPanel();
        }
    }

    private void AutoLogin()
    {
        if (User != null)
        {
            if (User.IsEmailVerified)
            {
                
            }
            else
            {
                SendEmailForVerification();
            }
        }
        else
        {
            UIManager.Instance.OpenLoginPanel();
        }
    }

    private void Auth_StateChanged(object sender, System.EventArgs e)
    {
        if (Auth.CurrentUser != null)
        {
            bool signedIn = User != Auth.CurrentUser && Auth.CurrentUser != null;

            if(!signedIn && User != null)
            {
                UIManager.Instance.OpenLoginPanel();
                ClearLoginInputFields();
            }

            User = Auth.CurrentUser;
        }
    }

    private void ClearLoginInputFields()
    {
        EmailLoginInputField.text = string.Empty;
        PasswordLoginInputField.text = string.Empty;
    }

    public void Login()
    {
        StartCoroutine(LoginAsync(EmailLoginInputField.text, PasswordLoginInputField.text));
    }

    public void LogOut()
    {
        if (Auth != null && User != null)
        {
            Auth.SignOut();
        }
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        Task<AuthResult> loginTask = Auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Login Failed! Because ";

            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage = "Login Failed";
                    break;
            }

            Debug.Log(failedMessage);
        }
        else
        {
            User = loginTask.Result.User;

            FirebaseRealtimeDataSaver.Instance.UserID = User.UserId;

            if (User.IsEmailVerified)
            {
                FirebaseRealtimeDataSaver.Instance.LoadData();
                SceneManager.LoadScene("MenuScene");
            }
            else
            {
                SendEmailForVerification();
            }
        }

        FirebaseRealtimeDataSaver.Instance.dataToSave.UserName = User.DisplayName;
    }

    public void Register()
    {
        StartCoroutine(RegisterAsync(NameRegisterInputField.text, EmailRegisterInputField.text,
            PasswordRegisterInputField.text, ConfirmPasswordRegisterInputField.text));
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (name == "")
        {
            Debug.LogError("User Name is empty");
        }
        else if (email == "")
        {
            Debug.LogError("email field is empty");
        }
        else if (PasswordRegisterInputField.text != ConfirmPasswordRegisterInputField.text)
        {
            Debug.LogError("Password does not match");
        }
        else
        {
            var registerTask = Auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogError(registerTask.Exception);

                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Registration Failed! Becuase ";

                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Wrong Password";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "Password is missing";
                        break;
                    default:
                        failedMessage = "Registration Failed";
                        break;
                }
            }
            else
            {
                User = registerTask.Result.User;

                UserProfile userProfile = new UserProfile { DisplayName = name };

                var updateProfileTask = User.UpdateUserProfileAsync(userProfile);

                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if (updateProfileTask.Exception != null)
                {
                    User.DeleteAsync();

                    Debug.LogError(updateProfileTask.Exception);

                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;

                    string failedMessage = "Profile update Failed! Becuase ";
                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            failedMessage += "Email is invalid";
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += "Wrong Password";
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += "Email is missing";
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += "Password is missing";
                            break;
                        default:
                            failedMessage = "Profile update Failed";
                            break;
                    }

                    Debug.Log(failedMessage);
                }
                else
                {
                    if (User.IsEmailVerified)
                    {
                        UIManager.Instance.OpenLoginPanel();
                    }
                    else
                    {
                        SendEmailForVerification();
                    }
                }
            }
        }
    }

    public void SendEmailForVerification()
    {
        StartCoroutine(SendEmailVerificationAsync());
    }

    private IEnumerator SendEmailVerificationAsync()
    {
        if(User != null)
        {
            Task SendEmailVerification = User.SendEmailVerificationAsync();
            yield return new WaitUntil(() =>  SendEmailVerification.IsCompleted);

            if (SendEmailVerification.Exception != null)
            {
                FirebaseException exception = SendEmailVerification.Exception.GetBaseException() as FirebaseException;
                AuthError error = (AuthError)exception.ErrorCode;

                string errorMessage = "An error occured";

                switch (error)
                {
                    case AuthError.Cancelled:
                        errorMessage = "Cancelled";
                        break;
                    case AuthError.TooManyRequests:
                        errorMessage = "TooManyRequests";
                        break;
                    case AuthError.InvalidRecipientEmail:
                        errorMessage = "InvalidRecipientEmail";
                        break;
                    default:
                        break;
                }

                Debug.Log(errorMessage);
            }
        }
    }
}
