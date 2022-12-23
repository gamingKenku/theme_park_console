using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace theme_park_console
{
    public enum SortType
    {
        NameSort,
        SessionTimeSort,
        TicketPriceSort
    }
    public class Node<T> where T : Attraction
    {
        public Node(T data)
        {
            Data = data;
            Next = null;
            Prev = null;
        }
        public T Data { get; set; }
        public Node<T> Next { get; set; }
        public Node<T> Prev { get; set; }
    }
    public class AttractionEnumerator<T> : IEnumerator<T> where T : Attraction
    {
        private Node<T> current, head;
        private bool startFlag;
        public AttractionEnumerator(Node<T> head)
        {
            current = null;
            startFlag = false;
            this.head = head;
        }
        public void Reset()
        {
            current = null;
            startFlag = false;
        }
        public void Dispose()
        {
            current = null;
            head = null;
            startFlag = false;
        }
        public bool MoveNext()
        {
            if (!startFlag)
            {
                current = head;
                startFlag = true;
            }
            else
            {
                if (current == null)
                    return false;

                current = current.Next;
            }
            return (current != null);
        }
        public T Current
        {
            get { return current.Data; }
        }
        object IEnumerator.Current
        {
            get { return current.Data; }
        }
    }
    public class AttractionLinkedList<T> : IEnumerable<T> where T : Attraction
    {
        private Logger logger;
        public Node<T> Head { get; set; }
        public Node<T> Tail { get; set; }
        public int count;
        public IEnumerator<T> GetEnumerator()
        {
            return new AttractionEnumerator<T>(Head);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new AttractionEnumerator<T>(Head);
        }
        public AttractionLinkedList()
        {
            Head = null;
            Tail = null;
            count = 0;
            logger = new Logger();
            logger.LogEvent += LoggerMethods.LogInConsole;
            logger.LogEvent += LoggerMethods.LogInFile;

            logger.InvokeLogEvent("Список был успешно создан.");
        }
        public void Add(T data)
        {
            Node<T> node = new Node<T>(data);

            if (Head == null && Tail == null) // если список пуст, первый элемент будет началом и концом
            {
                Head = node;
                Tail = node;
                count += 1;

                logger.InvokeLogEvent($"Пользователем был добавлен элемент в начало списка:\n{node.Data.GetInfo()}");

                return;
            }

            Tail.Next = node; // добавить в конец
            Tail = node;
            count += 1;

            logger.InvokeLogEvent($"Пользователем был добавлен элемент:\n{node.Data.GetInfo()}");
        }
        public bool Remove(int position)
        {
            if (position >= count && position < 0)
            {
                return false;
            }

            int i = 0;
            Node <T> current = Head;

            while (i != position)
            {
                current = current.Next;
                i++;
            }

            if (current.Prev == null)
            {
                Head = current.Next;
                Head.Prev = null;
                count--;

                logger.InvokeLogEvent($"Пользователем был удален элемент по индексу {position}:\n{current.Data.GetInfo()}");

                return true;
            }
            else if (current.Next == null)
            {
                Tail = current.Prev;
                Tail.Next = null;
                count--;

                logger.InvokeLogEvent($"Пользователем был удален элемент по индексу {position}:\n{current.Data.GetInfo()}");

                return true;
            }

            logger.InvokeLogEvent($"Пользователем был удален элемент по индексу {position}:\n{current.Data.GetInfo()}");

            current.Prev.Next = current.Next;
            count--;

            return true;
        }
        public void Clear()
        {
            Head = null;
            Tail = null;
            count = 0;

            logger.InvokeLogEvent("Список аттракционов был очищен пользователем.");
        }
        public void Sort(SortType type)
        {
            Func<Node<T>, Node<T>, bool> sortFunc = GetSortFunc(type);
            Node<T> node1 = Head;
            Node<T> node2 = Head.Next;
            bool sorted = false;

            if (node2 == Tail)
                if (sortFunc(node1, node2))
                {
                    Node<T> temp = Tail;
                    Tail = Head;
                    Head = temp;
                    return;
                }

            while (!sorted)
            {
                node1 = Head;
                node2 = Head.Next;
                sorted = true;

                while (node2 != null)
                {
                    if (sortFunc(node1, node2))
                    {
                        (node1.Data, node2.Data) = (node2.Data, node1.Data);
                        sorted = false;
                    }

                    node1 = node1.Next;
                    node2 = node2.Next;
                }
            }

            switch(type)
            {
                case SortType.NameSort:
                    logger.InvokeLogEvent("Список был отсортирован пользователем по названиям аттракционов.");
                    break;
                case SortType.TicketPriceSort:
                    logger.InvokeLogEvent("Список был отсортирован пользователем по ценам на билеты.");
                    break;
                case SortType.SessionTimeSort:
                    logger.InvokeLogEvent("Список был отсортирован пользователем по длительностям сессий.");
                    break;
            }
        }
        private bool NameCompare(Node<T> node, Node<T> next_node)
        {
            if (String.Compare(node.Data.name, next_node.Data.name) > 0)
                return true;
            else
                return false;
        }
        private bool SessionTimeCompare(Node<T> node, Node<T> next_node)
        {
            if (node.Data.session_time > next_node.Data.session_time)
                return true;
            else
                return false;
        }
        private bool TicketPriceCompare(Node<T> node, Node<T> next_node)
        {
            if (node.Data.ticket_price > next_node.Data.ticket_price)
                return true;
            else
                return false;
        }
        private Func<Node<T>, Node<T>, bool> GetSortFunc(SortType type)
        {
            switch (type)
            {
                case SortType.NameSort:
                    return NameCompare;
                case SortType.SessionTimeSort:
                    return SessionTimeCompare;
                case SortType.TicketPriceSort:
                    return TicketPriceCompare;
                default:
                    return NameCompare;
            }
        }
    }
}
