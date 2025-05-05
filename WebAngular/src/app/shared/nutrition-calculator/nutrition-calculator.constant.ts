import { CookingMethod } from '../../core/interfaces';

export const cookingMethods: CookingMethod[] = [
  {
    id: 'raw',
    name: 'Сирий (без обробки)',
    waterLossPercent: 0,
    caloriesModifier: 1,
    fatModifier: 1,
    carbsModifier: 1,
    proteinModifier: 1
  },
  {
    id: 'baked',
    name: 'Запікання',
    waterLossPercent: 25,
    caloriesModifier: 1.33,
    fatModifier: 0.95,
    carbsModifier: 1.05,
    proteinModifier: 0.92
  },
  {
    id: 'fried',
    name: 'Смаження',
    waterLossPercent: 30,
    caloriesModifier: 1.45,
    fatModifier: 1.2,
    carbsModifier: 1.1,
    proteinModifier: 0.88
  },
  {
    id: 'steamed',
    name: 'На пару',
    waterLossPercent: 10,
    caloriesModifier: 1.1,
    fatModifier: 0.98,
    carbsModifier: 1.02,
    proteinModifier: 0.97
  },
  {
    id: 'boiled',
    name: 'Варіння',
    waterLossPercent: 15,
    caloriesModifier: 1.2,
    fatModifier: 0.9,
    carbsModifier: 1.0,
    proteinModifier: 0.88
  },
  {
    id: 'stewed',
    name: 'Тушкування',
    waterLossPercent: 20,
    caloriesModifier: 1.25,
    fatModifier: 0.92,
    carbsModifier: 1.05,
    proteinModifier: 0.92
  },
  {
    id: 'grilled',
    name: 'Гриль',
    waterLossPercent: 35,
    caloriesModifier: 1.4,
    fatModifier: 0.8,
    carbsModifier: 1.05,
    proteinModifier: 0.9
  }
];
