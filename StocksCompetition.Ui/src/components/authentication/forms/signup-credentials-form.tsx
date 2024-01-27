import { useContext, useState } from 'react'
import AuthenticationFormProps from '@models/authentication/authentication-form-props'
import { AuthenticationFormType } from '@models/authentication/authentication-form-type'
import { SignupValidator } from '@utilities/signup-validator'
import { AuthenticationFormsContext } from '@context/authentication-forms-context'
import { ValidationErrors } from '@components/authentication/forms/ValidationErrors'

export const SignupCredentialsForm = ({ changeForm }: AuthenticationFormProps) => {
    const [emailErrors, setEmailErrors] = useState<string[]>([])
    const [passwordErrors, setPasswordErrors] = useState<string[]>([])
    const [confirmPasswordErrors, setConfirmPasswordErrors] = useState<string[]>([])
    
    const { formData, setFormData } = useContext(AuthenticationFormsContext)
    
    return (
        <div className='authentication-form-container'>
            <label htmlFor='email'>Email</label>
            <input 
                id='email'
                type='email'
                value={formData?.email} 
                onChange={e => setFormData({ email: e.target.value })}
                onBlur={async e => setEmailErrors(await SignupValidator.email(e.target.value))} />
            <ValidationErrors errors={emailErrors} />
            
            <label htmlFor='password'>Password</label>
            <input 
                id='password' 
                type='password'
                value={formData?.password}
                onChange={e => setFormData({ password: e.target.value })}
                onBlur={e => setPasswordErrors(SignupValidator.password(e.target.value))} />
            <ValidationErrors errors={passwordErrors} />
            
            <label htmlFor='confirm-password'>Confirm Password</label>
            <input 
                id='confirm-password'
                type='password'
                value={formData?.confirmPassword}
                onChange={e => setFormData({ confirmPassword: e.target.value })}
                onBlur={e => setConfirmPasswordErrors(SignupValidator.confirmPassword(formData?.password ?? '', e.target.value))} />
            <ValidationErrors errors={confirmPasswordErrors} />
            
            <footer>
                <button 
                    className='link'
                    onClick={() => changeForm(AuthenticationFormType.AuthenticationInputs)}>
                    Already have an account? Log in!
                </button>
                <button 
                    className='action'
                    onClick={() => changeForm(AuthenticationFormType.SignupDisplayDetails)}>Next</button>
            </footer>
        </div>
    )
}