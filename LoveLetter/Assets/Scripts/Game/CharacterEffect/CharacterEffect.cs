public abstract class CharacterEffect : ICharacterEffect
{
    public abstract CharacterType CharacterType { get; }
    public abstract bool DoEffect(PlayerScript player, int cardId);


}