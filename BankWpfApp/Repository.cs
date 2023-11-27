using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace BankWpfApp
{
    public interface IId
    {
        int UID { get; set; }
    }

    public class Repository<T> where T : IId
    {
        int currentNewUID = 0;
        string savePath = "";
        ObservableCollection<T> arr = new ObservableCollection<T>();

        public void SetArr(ObservableCollection<T> tmp)
        {
            arr = tmp;
        }
        public void SetSavePath(string path)
        {
            savePath = path;
        }
        public void SetCurrentNewUID(int id)
        {
            int maxUID = 0;
            foreach(T item in arr)
            {
                if (item.UID > maxUID)
                {
                    maxUID = item.UID;
                }
            }
            currentNewUID = (maxUID >= id) ? (maxUID + 1) : id;
        }
        /// <summary>
        /// Возвращает экземпляр T, или default(T) если такого экземпляра нет
        /// </summary>
        /// <param name="id">Позиция в базе данных</param>
        /// <returns>T</returns>
        public T this[int id]
        {
            get
            {
                foreach (T item in arr)
                {
                    if (item.UID == id)
                    {
                        return item;
                    }
                }
                return default(T);
            }
        }
        public T Add(T item)
        {
            item.UID = currentNewUID++;
            arr.Add(item);
            return item;
        }

        public void DelItem(T item)
        {
            arr.Remove(item);
        }

        public int Count => arr.Count;
        public ObservableCollection<T> AllItems => arr;

        public static Repository<T> LoadRepositoryFromFile(string path)
        {
            try
            {
                ObservableCollection<T> tempCol = new ObservableCollection<T>();
                // Создаем сериализатор на основе указанного типа 
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<T>));

                // Создаем поток для чтения данных
                Stream fStream = new FileStream(path, FileMode.Open, FileAccess.Read);

                // Запускаем процесс десериализации
                tempCol = xmlSerializer.Deserialize(fStream) as ObservableCollection<T>;

                // Закрываем поток
                fStream.Close();

                // Возвращаем результат
                Repository<T> ret = new Repository<T>();
                ret.SetArr(tempCol);
                return ret;
            }
            catch (Exception e)
            {
                return new Repository<T>();
            }
        }

        public void SaveRepositoryToFile(string Path)
        {
            // Создаем сериализатор на основе указанного типа 
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<T>));

            // Создаем поток для сохранения данных
            Stream fStream = new FileStream(Path, FileMode.Create, FileAccess.Write);

            // Запускаем процесс сериализации
            xmlSerializer.Serialize(fStream, arr);

            // Закрываем поток
            fStream.Close();
        }
        public void SaveRepositoryToFileForCusomSerializer(string Path, XmlSerializer xmlSerializer)
        {
            // Создаем сериализатор на основе указанного типа 
            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<T>));

            // Создаем поток для сохранения данных
            Stream fStream = new FileStream(Path, FileMode.Create, FileAccess.Write);

            // Запускаем процесс сериализации
            xmlSerializer.Serialize(fStream, arr);

            // Закрываем поток
            fStream.Close();
        }
    }
}
