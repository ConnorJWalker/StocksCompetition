export const AuthenticationOptions = () => {
    return (
        <div className='authentication-options-container'>
            <h1>Stocks Competition</h1>
            <section className='api-authentication-buttons'>
                <button>Sign Up</button>
                <button>Log In</button>
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
        </div>
    )
}