﻿using MediatR;
using sd.Jatek.Application.Querys;

namespace sd.Jatek.Application.QueryHandlers
{
    public class GetWordQueryHandler : IRequestHandler<GetWordQuery, string>
    {
        public async Task<string> Handle(GetWordQuery request, CancellationToken cancellationToken)
        {
            Random rnd = new();
            List<string> words = new();

            using (StreamReader reader = new("Files/Words.txt"))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    words.Add(line);
                }
            }

            int r = rnd.Next(words.Count);

            return words[r];
        }
    }
}
