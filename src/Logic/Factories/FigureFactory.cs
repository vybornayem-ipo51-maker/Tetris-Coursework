using Tetris.src.Logic.Abstract;
using Tetris.src.Logic.Tetrominoes;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Tetris.src.Logic.Factories;

public class FigureFactory
{
    private readonly Random random = new();
    private List<int> bag = new List<int>();

    public Tetromino CreateRandomFigure()
    {
        // Якщо мішок порожній, наповнюємо його всіма типами фігур (від 0 до 6)
        if (bag.Count == 0)
        {
            bag = Enumerable.Range(0, 7).ToList(); // Додаємо 0, 1, 2, 3, 4, 5, 6
            // Перемішуємо список (Fisher-Yates shuffle або простий OrderBy)
            bag = bag.OrderBy(x => random.Next()).ToList();
        }

        // Беремо першу фігуру з перемішаного мішка і видаляємо її звідти
        int value = bag[0];
        bag.RemoveAt(0);

        return value switch
        {
            0 => new ITetromino(),
            1 => new OTetromino(),
            2 => new TTetromino(),
            3 => new LTetromino(),
            4 => new JTetromino(),
            5 => new STetromino(),
            _ => new ZTetromino()
        };
    }
}