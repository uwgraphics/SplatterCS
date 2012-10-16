using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RangeTree
{

    public interface IPoint<C_Data>
    {
        float get_coordinate(int d);
        void init(float val);
        IComparer<C_Data> get_comp(int d);

    }

// class with the fractional cascading structure nodes
    public class Associated_structure_node<C_Data> : IComparable<Associated_structure_node<C_Data>>
        where C_Data : IPoint<C_Data>, new()
    {
        //constructors
        public Associated_structure_node()
        //: data(0), substructure(NULL), leftstructure(NULL), rightstructure(NULL), index(0)
        {
            data = default(C_Data);
            //substrtucture = null;
            leftstructure = null;
            rightstructure = null;
            index = 0;            
        }

        public Associated_structure_node(float input)
        //   : data(input), substructure(NULL), leftstructure(NULL), rightstructure(NULL), index(0) 
        {
            data = new C_Data();
            data.init(input);
            //substrtucture = null;
            leftstructure = null;
            rightstructure = null;
            index = 0;
        }


        public static bool operator <(Associated_structure_node<C_Data> right, Associated_structure_node<C_Data> left)
        {
            return right.data.get_coordinate(1) < left.data.get_coordinate(1);
        }

        public static bool operator >(Associated_structure_node<C_Data> right, Associated_structure_node<C_Data> left)
        {
            return right.data.get_coordinate(1) > left.data.get_coordinate(1);
        }

        public C_Data data;
        //Associated_structure<C_Data> substructure;
        public Associated_structure_node<C_Data> leftstructure;
        public Associated_structure_node<C_Data> rightstructure;
        public int index;

        public int CompareTo(Associated_structure_node<C_Data> other)
        {
            return this.data.get_coordinate(1).CompareTo(other.data.get_coordinate(1));
        }
    }

// class with the fractional cascading structure
    class Associated_structure<C_Data> where C_Data : IPoint<C_Data>, new()
    {
        int size;
        List<Associated_structure_node<C_Data>> assoc_array;

        public Associated_structure(int n)
        //: size(n), assoc_array(n) 
        {
            assoc_array = new List<Associated_structure_node<C_Data>>(n);
            size = n;
            for (int i = 0; i < n; i++)
            {
                assoc_array.Add(new Associated_structure_node<C_Data>());
                assoc_array[i].index = i;
            }
        }

        void delete_structure()
        {
            assoc_array.Clear();
        }

        //void print() const{
        //  typename Associated_vector::const_iterator p;
        //  p = assoc_array.begin();
        //  std::cout << "[";
        //  //std::cout << p->data;
        //  while (p != assoc_array.end()){
        //    std::cout << " " << p->data.get_coordinate(1);
        //    std::cout << "[";
        //    if (p->leftstructure != NULL)
        //      std::cout << p->leftstructure->data.get_coordinate(1) << " (" << p->leftstructure->index << ") "; 
        //    else std::cout << "NULL ";
        //    if (p->rightstructure != NULL)
        //      std::cout << p->rightstructure->data.get_coordinate(1) << " (" << p->rightstructure->index << ") ";
        //    else std::cout << "NULL";
        //    std::cout << "]";
        //    p++;
        //  }
        //  std::cout << "]";
        //}

        //destructor
        ~Associated_structure()
        {
            assoc_array.Clear();
            //std::cout << "~Associated_structure():an associated structure deleted!" 
            //          << std::endl;
        }

        public void build_struct(List<C_Data> array, int begin, int end, int d)
        {
            int i = 0;
            int j = begin;
            while (j != end)
            {
                assoc_array[i].data = array[j];
                j++;
                i++;
            }
        }

        public void put_frac_casc(int node, Associated_structure_node<C_Data> frac_node, bool left)
        {
            if (left)
            {
                assoc_array[node].leftstructure = frac_node;
                //assoc_array[node].leftstructure_index = index;
            }
            else
            {
                assoc_array[node].rightstructure = frac_node;
                //assoc_array[node].rightstructure_index = index;
            }
        }

        public Associated_structure_node<C_Data> get_frac_casc(int node)
        {
            return assoc_array[node];
        }

        //TODO: clear s 
        public Associated_structure_node<C_Data> assoc_binary_search(float i)
        {

            Associated_structure_node<C_Data> s = new Associated_structure_node<C_Data>(i);
            int index = assoc_array.BinarySearch(s);
            if (index < 0)
            {
                index = ~index;
            }
            return assoc_array[index];
        }

        public int get_end(){
          return assoc_array.Last().index;
        }

        public void report_structure(Associated_structure_node<C_Data> assoc_v,
                              List<C_Data> result2,
                              float y_2)
        {
            int index = assoc_v.index;
            while (index != assoc_array.Count && assoc_array[index].data.get_coordinate(1) <= y_2)
            {
                result2.Add(assoc_array[index].data);
                index++;
            }
        }

        public void push_data(C_Data input_data, int i, int d)
        {
            assoc_array[i].data = input_data;
        }
    }

//
// class with definitions of range trees nodes
//
class Layered_range_tree_node<C_Data,T> where C_Data : IPoint<C_Data>, new() {
  
  //template <C_Data,T> friend class Layered_range_tree;
    // constructor   
    public Layered_range_tree_node()    
    {
        data = 0;
        substructure = default(T);
    }
    
    // not used
    //Associated_structure<C_Data> get_substructure(){
    //  return static_cast<Associated_structure<C_Data>*>(substructure);
    //}

  
    public float data;
    public T substructure;
    public List<C_Data> dData;
}

//
// the class with the definitions of the last dimension tree
//
public class Last_range_tree<C_Data> where C_Data : IPoint<C_Data>, new()
{

    int size;
    List<Layered_range_tree_node<C_Data, Associated_structure<C_Data>>> main_tree;

    C_Data dummy = new C_Data();
    //typedef Layered_range_tree_node<C_Data, Associated_structure<C_Data> > Tree_node;
    //typedef typename std::vector<Tree_node> Tree_vector;
    //typedef typename std::vector<C_Data>::iterator Input_data;


    int parent(int i)
    {
        return (i != 0 ? (i - 1) / 2 : -1);
    }

    int left_child(int i)
    {
        return (2 * i + 1 <= size ? 2 * i + 1 : -1);
    }

    int right_child(int i)
    {
        return (2 * i + 2 <= size ? 2 * i + 2 : -1);
    }

    bool is_leaf(int i)
    {
        return (i >= size / 2 ? true : false);
    }


    //constructors
    public Last_range_tree(int n)
    //: size(2*n-1), main_tree(2*n-1) 
    {
        size = 2 * n - 1;
        main_tree = new List<Layered_range_tree_node<C_Data, Associated_structure<C_Data>>>(size);
        for (int i = 0; i < size; i++)
        {
            main_tree.Add(new Layered_range_tree_node<C_Data, Associated_structure<C_Data>>());
        }
    }

    //destructor
    ~Last_range_tree()
    {
        for (int i = 0; i < size; i++)
        {
            //main_tree[i].substructure.delete_structure();
        }
        main_tree.Clear();
    }

    void delete_tree()
    {
        for (int i = 0; i < size; i++)
        {
            //main_tree[i].substructure->delete_structure();
        }
        main_tree.Clear();
    }

    // print tree's vector
    //void print_tree_vector() const {
    //  typename Tree_vector::const_iterator p;
    //  p = main_tree.begin()+size/2;
    //  while (p != main_tree.end()){
    //    std::cout << p->data << " ";
    //    p->substructure->print();
    //    p++; 
    //  }
    //}//print_tree()


    //print the main tree
    //void print_tree() const {
    //  typename Tree_vector::const_iterator p;
    //  p = main_tree.begin();
    //  std::cout << std::endl << "last tree:" << std::endl;
    //  std::cout << "* ";
    //  int i = 1, j = 0;
    //  while (p != main_tree.end()){
    //    std::cout << p->data << "{";
    //    p->substructure->print();
    //    std::cout << "} ";
    //    if (i++ == pow(2.0,j)) {
    //  std::cout << std::endl << "|-";
    //      //p->substructure->print();
    //      std::cout << std::endl << "| ";
    //      i = 1;
    //      j++;
    //    }//if
    //    p++;


    //  }//while
    //  std::cout << "---end_of_tree---" << std::endl;
    //}//print_tree()

    // builds the main tree from the container, which the iterators index   
    public void build_tree(List<C_Data> array, int begin, int end,
                    int n, int vector_place, int d)
    {

        if (n == 1)
        { //leaf
            main_tree[vector_place].data = array[begin].get_coordinate(d);
            main_tree[vector_place].dData = new List<C_Data>(1);
            main_tree[vector_place].dData.Add(array[begin]);
            main_tree[vector_place].substructure = new Associated_structure<C_Data>(n);
            main_tree[vector_place].substructure.build_struct(array, begin, end, d - 1);
            //main_tree[vector_place].substructure->print(); 
        }
        else
        {

            main_tree[vector_place].substructure = new Associated_structure<C_Data>(n);

            main_tree[vector_place].data = array[begin + (n / 2 - 1)].get_coordinate(d);
            build_tree(array, begin, end - (n / 2),
                       n / 2, left_child(vector_place), d);
            build_tree(array, begin + (n / 2), end,
                       n / 2, right_child(vector_place), d);

            main_tree[vector_place].dData = new List<C_Data>(n);

            merge(main_tree[left_child(vector_place)].dData,
                main_tree[right_child(vector_place)].dData,
                main_tree[vector_place].dData, dummy.get_comp(1));


            main_tree[vector_place].substructure.build_struct(main_tree[vector_place].dData, 0, main_tree[vector_place].dData.Count, d - 1);

            // FRACTIONAL CASCADING //
            //
            C_Data median = array[begin + (n / 2 - 1)];

            int i = 0, j = 0, k = 0;
            foreach (var data in main_tree[vector_place].dData)
            {
                if (data.get_coordinate(d) <= median.get_coordinate(d))
                {
                    main_tree[vector_place].substructure.put_frac_casc(
                         k,
                         main_tree[left_child(vector_place)].substructure.get_frac_casc(i),
                         true);
                    if (j != n / 2)
                    {//if not, then the right structure is full so point to null 
                        main_tree[vector_place].substructure.put_frac_casc(
                           k,
                           main_tree[right_child(vector_place)].substructure.get_frac_casc(j),
                           false);
                    }
                    i++; k++;
                }
                else
                {
                    if (i != n / 2)
                    {//if not, then the left structure is full so point to null
                        main_tree[vector_place].substructure.put_frac_casc(
                           k,
                           main_tree[left_child(vector_place)].substructure.get_frac_casc(i),
                           true);
                    }
                    main_tree[vector_place].substructure.put_frac_casc(
                         k,
                         main_tree[right_child(vector_place)].substructure.get_frac_casc(j),
                         false);
                    j++; k++;
                }
            }//for 

        }//else not leaf    
    }

    //last tree prebuild not used
    void pre_build_tree(List<C_Data> input)
    {
        //std::sort(input.begin(), input.end(), C_Data::get_comp(d));
        //std::sort(input.begin(), input.end(), T::ycompare);

    }

    private void merge(List<C_Data> one, List<C_Data> two, List<C_Data> dest, IComparer<C_Data> comp)
    {
        int i1 = 0;
        int i2 = 0;

        while (i1 < one.Count && i2 < two.Count)
        {
            if (comp.Compare(one[i1], two[i2]) <= 0)
            {
                dest.Add(one[i1]);
                i1++;
            }
            else
            {
                dest.Add(two[i2]);
                i2++;
            }
        }
        if (i1 < one.Count)
        {
            dest.AddRange(one.GetRange(i1, one.Count - i1));
        }
        if (i2 < two.Count)
        {
            dest.AddRange(two.GetRange(i2, two.Count - i2));
        }
    }
    // returns the index of the node in which 
    // the paths (from root) of x_1, x_2 splits 
    int find_split_node(double x_1, double x_2)
    {
        int split = 0;
        while ((!is_leaf(split)) && (x_1 > main_tree[split].data
                           || x_2 <= main_tree[split].data))
        {
            split = main_tree[split].data >= x_1
                  ? left_child(split) : right_child(split);
        }
        return split;
    }

    // reports the subtree of the node with index v
    void report_subtree(int v, List<float> result)
    {
        if (!is_leaf(v))
        {
            report_subtree(left_child(v), result);
            report_subtree(right_child(v), result);
        }
        else
            result.Add(main_tree[v].data);
    }


    // performs a range query in the last tree
    public void range_query(C_Data from, C_Data to, int d,
                       List<C_Data> result)
    {
        float x_1 = from.get_coordinate(d);
        float x_2 = to.get_coordinate(d);
        float y_1 = from.get_coordinate(d - 1);
        float y_2 = to.get_coordinate(d - 1);
        int split = find_split_node(x_1, x_2);

        var assoc_split = main_tree[split].substructure.assoc_binary_search(y_1);
        if (assoc_split.index != main_tree[split].substructure.get_end())
        {
            //the answer of binary search is inside the index
            Associated_structure_node<C_Data> assoc_v = assoc_split;

            if (is_leaf(split))
            {
                if (x_1 <= main_tree[split].data && main_tree[split].data <= x_2)
                {
                    main_tree[split].substructure.report_structure(assoc_v, result, y_2);
                }
            }
            else
            {
                //left path
                int v = left_child(split);
                assoc_v = assoc_split;
                if (assoc_v.leftstructure != null)
                    assoc_v = assoc_v.leftstructure;
                while (!is_leaf(v))
                {
                    if (x_1 <= main_tree[v].data)
                    {
                        if (assoc_v.rightstructure != null)
                            main_tree[right_child(v)].substructure.
                                    report_structure(assoc_v.rightstructure, result, y_2);
                        v = left_child(v);
                        if (assoc_v.leftstructure != null)
                            assoc_v = assoc_v.leftstructure;
                    }
                    else
                    {
                        v = right_child(v);
                        if (assoc_v.rightstructure != null)
                            assoc_v = assoc_v.rightstructure;
                    }
                }
                if (x_1 <= main_tree[v].data && main_tree[v].data <= x_2)
                {
                    main_tree[v].substructure.report_structure(assoc_v, result, y_2);
                }

                //right path
                v = right_child(split);
                assoc_v = assoc_split;
                if (assoc_v.rightstructure != null)
                    assoc_v = assoc_v.rightstructure;
                while (!is_leaf(v))
                {
                    if (main_tree[v].data <= x_2)
                    {
                        if (assoc_v.leftstructure != null)
                            main_tree[left_child(v)].substructure.
                                          report_structure(assoc_v.leftstructure, result, y_2);

                        v = right_child(v);
                        if (assoc_v.rightstructure != null)
                            assoc_v = assoc_v.rightstructure;
                    }
                    else
                    {
                        v = left_child(v);
                        if (assoc_v.leftstructure != null)
                            assoc_v = assoc_v.leftstructure;
                    }
                }
                if (x_1 <= main_tree[v].data && main_tree[v].data <= x_2)
                {
                    main_tree[v].substructure.report_structure(assoc_v, result, y_2);
                }
            }//else
        }
        else
        {
            System.Console.WriteLine("error: binary search out of borders");
        }
    }//range_query
}

////
//// the class with the definitions of the range trees
////
//template <class C_Data, class T>
//class Layered_range_tree {

//  typedef Layered_range_tree_node<C_Data,T> Tree_node;
//  typedef typename std::vector<Tree_node> Tree_vector;
//  typedef typename std::vector<C_Data>::iterator Input_data;
   
//  public:
//    //constructors
//    Layered_range_tree(int n) : size(2*n-1), main_tree(2*n-1) { }
    
//    //destructor
//    ~Layered_range_tree() {
//      this->delete_tree();
//    }
    
//    void delete_tree() {
//      for (int i=0; i < size; i++) {
//        main_tree[i].substructure->delete_tree();
//      }
//      main_tree.clear(); 
//    }
    
//    // print tree's vector
//    void print_tree_vector() const {
//      typename Tree_vector::const_iterator p;
//      p = main_tree.begin()+size/2;
//      while (p != main_tree.end()){
//        std::cout << p++->data << " ";
//      }
//    }//print_tree()
    
    
//    //print the main tree
//    void print_tree() const {
//      typename Tree_vector::const_iterator p;
//      p = main_tree.begin();
//      std::cout << std::endl << "main(or)high tree:" << std::endl;
//      std::cout << "* ";
//      int i = 1, j = 0;
//      while (p != main_tree.end()){
//        std::cout << p->data << "{ ";
//        p->substructure->print_tree();
//        std::cout << "} ";
//        if (i++ == pow(2.0,j)) {
//      std::cout << std::endl << "|-";
//          i = 1;
//          j++;
//        }//if
//        p++;
//      }//while
//      std::cout << "---end_of_tree---" << std::endl;
//    }//print_tree()
    
//    // builds the main tree from the container, which the iterators index   
//    void build_tree(Input_data begin,
//                    Input_data end,
//                    int n, int vector_place, const int d) {

//       if (n == 1) { //leaf
//         main_tree[vector_place].data = (*begin).get_coordinate(d);
//         main_tree[vector_place].dData = new std::vector<C_Data>(1);
//         (*main_tree[vector_place].dData)[0] = (*begin);
         
         
//         main_tree[vector_place].substructure = new T(n);
//         main_tree[vector_place].substructure
//                 ->build_tree(main_tree[vector_place].dData->begin(), 
//                              main_tree[vector_place].dData->end(), 
//                              n, 0, d-1);
//       } 
//       else {
//           if (d > 2){
//             main_tree[vector_place].substructure = new T(n);
//           }

//         build_tree(begin, end-(n/2),
//                    n/2, left_child(vector_place), d);
//         build_tree(begin+(n/2), end,
//                    n/2, right_child(vector_place), d);

//         main_tree[vector_place].data = (*(begin+(n/2-1))).get_coordinate(d);
         
//         main_tree[vector_place].dData = new std::vector<C_Data> (n);
         
//         std::merge(main_tree[left_child(vector_place)].dData->begin(),
//                    main_tree[left_child(vector_place)].dData->end(),
//                    main_tree[right_child(vector_place)].dData->begin(),
//                    main_tree[right_child(vector_place)].dData->end(),  
//                    main_tree[vector_place].dData->begin(), 
//                    *(C_Data::get_comp(d-2)));
         
//         main_tree[left_child(vector_place)].dData->clear();
//         main_tree[right_child(vector_place)].dData->clear();
         
//         main_tree[vector_place].substructure 
//                 ->build_tree(main_tree[vector_place].dData->begin(), 
//                              main_tree[vector_place].dData->end(), 
//                              n, 0, d-1);
//       }    
//    }


//    void pre_build_tree(std::vector<C_Data>& input, int d) {
//      std::sort(input.begin(), input.end(), *(C_Data::get_comp(d-1)));
//    } 
  
//  private:  
//    // returns the index of the node in which 
//    // the paths (from root) of x_1, x_2 splits 
//    int find_split_node(double x_1, double x_2) {
//      int split = 0;
//      while ((!is_leaf(split)) && (x_1 > main_tree[split].data 
//                         || x_2 <= main_tree[split].data)) {
//        split = main_tree[split].data >= x_1 
//              ? left_child(split) : right_child(split);
//      }
//      return split;
//    }
     
//    // reports the subtree of the node with index v
//    void report_subtree(int v, std::vector<double>& result) {
//      if (!is_leaf(v)) { 
//        report_subtree(left_child(v), result);
//        report_subtree(right_child(v), result);
//      }
//      else
//        result.push_back(main_tree[v].data);
//    }
  
//  public:
//    // performs a range query
//    void range_query(C_Data from, C_Data to, int d, 
//                     std::vector<C_Data>& result) {
//      double x_1 = from.get_coordinate(d);
//      double x_2 = to.get_coordinate(d);
//      int split = find_split_node(x_1, x_2);
 
//      if (is_leaf(split)) { 
//        if (x_1 <= main_tree[split].data && main_tree[split].data <= x_2) {
//            if (d != 1)
//          main_tree[split].substructure->range_query(from, to, d-1, result); 
//        }
//      }
//      else {
//        //left path
//        int v = left_child(split);
//        while (!is_leaf(v)) {
//          if (x_1 <= main_tree[v].data) {
//            if (d != 1)
//            main_tree[right_child(v)].substructure->
//                      range_query(from, to, d-1, result);
//            v = left_child(v);
//          } 
//          else {
//            v = right_child(v); 
//          }          
//        }
//        if (x_1 <= main_tree[v].data && main_tree[v].data <= x_2) {
//            if (d != 1)
//          main_tree[v].substructure->range_query(from, to, d-1, result); 
//        }
//        //right path
//        v = right_child(split);
//        while (!is_leaf(v)) {
//          if (main_tree[v].data <= x_2) {
//            if (d != 1)
//            main_tree[left_child(v)].substructure->
//                              range_query(from, to, d-1, result);
//            v = right_child(v);
//          } 
//          else {
//            v = left_child(v);
//          }           
//        }
//        if (x_1 <= main_tree[v].data && main_tree[v].data <= x_2) {
//            if (d != 1)
//          main_tree[v].substructure->range_query(from, to, d-1, result);
//        }
//      }//else
//    }//range_query
  
// private:
//    // functions for index arithmetic
//    int parent(int i) const {
//      return (i != 0 ? (i-1)/2 : -1);
//    }
    
//    int left_child(int i) const {
//      return (2*i+1 <= size ? 2*i+1 : -1);
//    }
    
//    int right_child(int i) const {
//      return (2*i+2 <= size ? 2*i+2 : -1);
//    }
    
//    bool is_leaf(int i) const {
//      return (i >= size/2 ? true : false);
//    }
//  private:
//    int size;
//    Tree_vector main_tree; 
//};



}
