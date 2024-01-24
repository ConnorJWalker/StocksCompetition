export const randomColour = () => {
    const availableCharacters = "0123456789abcdef"

    let colour = '#'
    for (let i = 0; i < 6; i++) {
        colour += availableCharacters[Math.floor(Math.random() * availableCharacters.length)]
    }

    return colour
}