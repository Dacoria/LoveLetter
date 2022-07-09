public abstract class CharacterEffect : ICharacterEffect
{
    public abstract CharacterType CharacterType { get; }    
    public abstract bool CanDoEffect(PlayerScript player, int cardId);
    public abstract bool DoEffect(PlayerScript player, int cardId);
}