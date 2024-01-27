import { useContext, useState } from 'react'
import AuthenticationFormProps from '@models/authentication/authentication-form-props'
import { AuthenticationFormType } from '@models/authentication/authentication-form-type'
import { AuthenticationFormsContext } from '@context/authentication-forms-context'
import { ValidationErrors } from '@components/authentication/forms/ValidationErrors'
import { SignupValidator } from '@utilities/signup-validator'

export const DisplayDetailsForm = ({ changeForm }: AuthenticationFormProps) => {
    const [usernameErrors, setUsernameErrors] = useState<string[]>([])
    const [displayNameErrors, setDisplayNameErrors] = useState<string[]>([])
    
    const { formData, setFormData } = useContext(AuthenticationFormsContext)

    return (
        <div className='authentication-form-container'>
            <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }} className='profile-picture'>
                <p style={{ color: 'white' }}>Profile Picture</p>
            </div>
            
            <label htmlFor='username'>Username</label>
            <input 
                id='username'
                type='text'
                value={formData?.username}
                onChange={e => setFormData({ username: e.target.value })}
                onBlur={async e => setUsernameErrors(await SignupValidator.username(e.target.value))} />
            <ValidationErrors errors={usernameErrors} />
            
            <label htmlFor='display-name'>Display Name</label>
            <input 
                id='display-name'
                type='text'
                value={formData?.displayName}
                onChange={e => setFormData({ displayName: e.target.value })}
                onBlur={e => setDisplayNameErrors(SignupValidator.displayName(e.target.value))} />
            <ValidationErrors errors={displayNameErrors} />
            
            <label htmlFor='display-colour'>Display Colour</label>
            <label 
                tabIndex={0}
                className='display-colour-field' 
                htmlFor='display-colour' 
                style={{ backgroundColor: formData?.displayColour, marginTop: 0 }}>
                
                <input
                    hidden
                    id='display-colour'
                    type='color'
                    value={formData?.displayColour}
                    onChange={ e => setFormData({ displayColour: e.target.value }) } />
            </label>
            
            <footer>
                <button 
                    className='link'
                    onClick={() => changeForm(AuthenticationFormType.SignupCredentials)}>
                    Back to credentials form
                </button>
                <button
                    className='action'
                    onClick={() => changeForm(AuthenticationFormType.SignupApiKey)}>
                    Next
                </button>
            </footer>
        </div>
    )
}