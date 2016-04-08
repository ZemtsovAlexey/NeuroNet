namespace test
{
    class Sloy
    {
        public Neuron[] Neironu;
        int n = 1;
        double so = 0.01; // скорость обучения

        public Sloy() //создание сети
        {
            n = 1;
            Neironu = new Neuron[n];
            for (int i = 0; i < n; i++)
            {
                Neironu[i] = new Neuron();
                Neironu[i].setWeight(); //установка весов по умолчанию
            }
        }

        public Sloy(int n, int vhodi) // перегрузка функции (с входными параметрами)
        {
            this.n = n;
            Neironu = new Neuron[n];
            for (int i = 0; i < n; i++)
            {
                Neironu[i] = new Neuron(vhodi);
                Neironu[i].setWeight();
            }
        }

        public void Activate()
        {
            for (int i = 0; i < n; i++)
            {
                Neironu[i].Activate();
            }
        }

        public void Learning(int k)//обучение к=0,1,2 - номер нейрона который должен выиграть
        {
            for (int i = 0; i < n; i++)//обходим нейроны все
            {
                if (i == k)
                {
                    for (int j = 0; j < Neironu[i].n; j++)
                    {
                        Neironu[i].weight[j] += so * (0.75 - Neironu[i].getOutput()) * Neironu[i].input[j];//пересчет весов
                    }
                }
                else
                {
                    for (int j = 0; j < Neironu[i].n; j++)
                    {
                        Neironu[i].weight[j] += so * (0.25 - Neironu[i].getOutput()) * Neironu[i].input[j];
                    }
                }
            }
        }

        //вес += скорость_обучения * разница_между_выходом_нейрона_и_его_нужным_значением * вход_сети
        public double[] poluchves()// получение весов для сохранения
        {
            int kol = Neironu[0].n * n;
            double[] tmp = new double[kol];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < Neironu[i].n; j++)
                {
                    tmp[i * n + j] = Neironu[i].weight[j];
                }
            }
            return tmp;
        }

        public void ustanves(double[] tmp) // установка весов после загрузки из файла
        {
            int kol = Neironu[0].n * n;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < Neironu[i].n; j++)
                {
                    Neironu[i].weight[j] = tmp[i * n + j];
                }
            }
        }
    }
}