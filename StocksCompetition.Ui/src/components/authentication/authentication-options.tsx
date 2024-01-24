import AuthenticationFormProps from '@models/authentication/authentication-form-props'
import { AuthenticationFormType } from '@models/authentication/authentication-form-type'

export const AuthenticationOptions = ({ changeForm }: AuthenticationFormProps) => {
    return (
        <>
            <section className='api-authentication-buttons'>
                <button onClick={() => changeForm(AuthenticationFormType.SignupCredentials)}>Sign Up</button>
                <button 
                    className='action' 
                    onClick={() => changeForm(AuthenticationFormType.Login)}>
                    Log In
                </button>
            </section>

            <div className='divider'>
                <span></span>
                <p>OR</p>
                <span></span>
            </div>

            <section className='oauth-buttons'>
                <button>
                    <img src='/oauth-icons/microsoft.svg' alt='google logo'/>
                    <p>Sign in with Microsoft</p>
                </button>
                <button>
                    <img src='/oauth-icons/google-light.svg' alt='google logo'/>
                    <p>Sign in with Google</p>
                </button>
                <button>
                    <img src="/oauth-icons/facebook.png" alt="google logo"/>
                    <p>Sign in with Facebook</p>
                </button>
            </section>
        </>
    )
}