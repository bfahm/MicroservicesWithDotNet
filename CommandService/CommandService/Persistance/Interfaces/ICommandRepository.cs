using CommandService.Models;
using System.Collections.Generic;

namespace CommandService.Persistance.Interfaces
{
    public interface ICommandRepository
    {
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);
        bool SaveChanges();
    }
}
