export const ApiKeyInput = () => {
    return (
        <div className='api-key-input'>
            <select>
                <option value='isa'>ISA</option>
                <option value='gia'>GIA</option>
                <option value='cfd'>CFD</option>
            </select>
            <input type='text'/>
        </div>
    )
}