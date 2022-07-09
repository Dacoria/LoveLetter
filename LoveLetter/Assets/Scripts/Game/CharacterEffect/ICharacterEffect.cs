public interface ICharacterEffect
{
    public CharacterType CharacterType { get; }
    public bool DoEffect(PlayerScript player, int cardId);
    public bool CanDoEffect(PlayerScript player, int cardId);
}