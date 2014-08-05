using System.Linq;
using Microsoft.Phone.Shell;

namespace UsedParts.PhoneServices.Impl
{
    public class TileManager : ITileManager
    {
        public void SetPrimaryCount(int i)
        {
            var tile = ShellTile.ActiveTiles.FirstOrDefault();
            if (null == tile) 
                return;
            var data = new StandardTileData { Count = i };
            tile.Update(data);
        }
    }
}
