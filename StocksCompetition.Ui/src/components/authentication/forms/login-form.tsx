import AuthenticationFormProps from '@models/authentication/authentication-form-props'
import { AuthenticationFormType } from '@models/authentication/authentication-form-type'

export const LoginForm = ({ changeForm }: AuthenticationFormProps) => {
    return (
        <div className='authentication-form-container'>
            <label htmlFor='username'>Username or Email</label>
            <input id='username' type='text'/>
            <label htmlFor='password'>Password</label>
            <input id='password' type='password'/>

            <footer>
                <button onClick={() => changeForm(AuthenticationFormType.AuthenticationInputs)}>Back</button>
                <button onClick={() => changeForm(AuthenticationFormType.SignupDisplayDetails)}>Log In</button>
            </footer>
        </div>
    )
}