
namespace RGN.Sample.UI
{
    public abstract class AbstractPopup : AbstractPanel
    {
        public virtual bool IsActive()
        {
            return gameObject.activeSelf;
        }
    }
}