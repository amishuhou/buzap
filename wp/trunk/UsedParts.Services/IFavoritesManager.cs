using System;
using System.Collections.Generic;

namespace UsedParts.Services
{
    public interface IFavoritesManager<T> where T : class
    {
        void Add(T item);
        void Remove(T item);
        bool IsFavorite(T item);
        IList<T> GetAll();

        event EventHandler<FavoritesEventArgs<T>> ItemChanged;
    }

    public class FavoritesEventArgs<T> : EventArgs where T : class
    {
        public T Item { get; set; }
        public bool IsFavorite { get; set; }
    }
}
