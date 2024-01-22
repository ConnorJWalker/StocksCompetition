import { useState } from 'react'
import { AuthenticationFormType } from '@models/authentication/authentication-form-type'
import { AuthenticationOptions } from '@components/authentication/authentication-options'
import { LoginForm } from '@components/authentication/forms/login-form'
import { SignupCredentialsForm } from '@components/authentication/forms/signup-credentials-form'

export const AuthenticationInputs = () => {
    const [currentForm, setCurrentForm] = useState(AuthenticationFormType.AuthenticationInputs)
    
    const changeForm = (formType: AuthenticationFormType) => setCurrentForm(formType)
    
    const forms = {
        [AuthenticationFormType.AuthenticationInputs]: <AuthenticationOptions changeForm={changeForm} />,
        [AuthenticationFormType.Login]: <LoginForm changeForm={changeForm} />,
        [AuthenticationFormType.SignupCredentials]: <SignupCredentialsForm changeForm={changeForm} />,
        [AuthenticationFormType.SignupDisplayDetails]: <h1>TODO</h1>,
        [AuthenticationFormType.SignupApiKey]: <h1>TODO</h1>,
    }
    
    return (
        <div className='authentication-inputs-container'>
            <h1>Stocks Competition</h1>
            { forms[currentForm] }
        </div>
    )
}