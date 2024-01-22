import AuthenticationFormProps from '@models/authentication/authentication-form-props'
import { AuthenticationFormType } from '@models/authentication/authentication-form-type'

export const SignupCredentialsForm = ({ changeForm }: AuthenticationFormProps) => {
    return (
        <div className='authentication-form-container'>
            <label htmlFor='username'>Email</label>
            <input id='username' type='email'/>
            <label htmlFor='password'>Password</label>
            <input id='password' type='password'/>
            <label htmlFor='confirm-password'>Confirm Password</label>
            <input id='confirm-password' type='password'/>

            <footer>
                <button onClick={() => changeForm(AuthenticationFormType.AuthenticationInputs)}>Back</button>
                <button onClick={() => changeForm(AuthenticationFormType.SignupDisplayDetails)}>Next</button>
            </footer>
        </div>
    )
}