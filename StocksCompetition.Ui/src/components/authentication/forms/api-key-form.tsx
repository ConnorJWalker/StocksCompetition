import AuthenticationFormProps from '@models/authentication/authentication-form-props.ts'
import { AuthenticationFormType } from '@models/authentication/authentication-form-type.ts'

export const ApiKeyForm = ({ changeForm }: AuthenticationFormProps) => {
    const signupClick = () => {
        alert('sign up')
    }
    
    const continueWithoutKeyClick = () => {
        alert('continue without key')
    }
    
    return (
        <div className='authentication-form-container'>
            <h3>Trading 212 Api Key</h3>
            <ul className='api-key-instructions'>
                <li>Open Trading 212 into invest account (not ISA or CFD)</li>
                <li>Click Settings -&gt; Api (Beta) -&gt; Generate API Key</li>
                <li>Set all permission toggle (except Orders - Execute) to on</li>
                <li>Click Generate key and copy to the text area below</li>
            </ul>

            <textarea rows={5}></textarea>
            
            <footer>
                <button
                    className='link'
                    onClick={() => changeForm(AuthenticationFormType.SignupDisplayDetails)}>
                    Back to display details
                </button>
                <button
                    className='action'
                    onClick={signupClick}>
                    Sign Up
                </button>
            </footer>

            <div className='without-key'>
                <button
                    className='link'
                    onClick={continueWithoutKeyClick}>
                    Continue without key
                </button>
            </div>
        </div>
    )
}