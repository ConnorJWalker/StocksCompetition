interface props { errors: string[] }

export const ValidationErrors = ({ errors }: props) => (
    <ul className='validation-errors'>
        { errors.map((error, index) => <li key={index}>{ error }</li>) }
    </ul>
)