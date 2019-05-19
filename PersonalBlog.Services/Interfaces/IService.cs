using System.Collections.Generic;

namespace PersonalBlog.Services.Interfaces
{
    public interface IService<TDto, TFilter>
    {
        TDto Get(string id);
        IEnumerable<TDto> Get(TFilter filter);
        void Add(TDto dto);
        void Remove(string id);
        void Update(TDto dto);
    }
}
