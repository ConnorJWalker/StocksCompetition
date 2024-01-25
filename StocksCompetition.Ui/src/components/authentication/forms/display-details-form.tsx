import AuthenticationFormProps from '@models/authentication/authentication-form-props.ts'
import { AuthenticationFormType } from '@models/authentication/authentication-form-type.ts'
import { useState } from 'react'
import { randomColour } from '@utilities/random-colour.ts'

export const DisplayDetailsForm = ({ changeForm }: AuthenticationFormProps) => {
    const [colour, setColour] = useState<string>(randomColour())
    
    return (
        <div className='authentication-form-container'>
            <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }} className='profile-picture'>
                <p style={{ color: 'white' }}>Profile Picture</p>
            </div>
            
            <label htmlFor='username'>Username</label>
            <input id='username' type='text'/>
            <label htmlFor='display-name'>Display Name</label>
            <input id='display-name' type='text'/>
            
            <label htmlFor='display-colour'>Display Colour</label>
            <label className='display-colour-field' htmlFor='display-colour' style={{ backgroundColor: colour }}>
                <input
                    hidden 
                    id='display-colour'
                    type='color'
                    value={colour}
                    onChange={e => setColour(e.target.value)} />
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