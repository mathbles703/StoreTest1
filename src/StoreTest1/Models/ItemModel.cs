using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StoreTest1.Models
{
    public class ItemModel
    {
        /// <summary>
        ///  MenuItemModel - Model class representing a MenuItem
        ///     Author:     Evan Lauersen
        ///     Date:       Created: Feb 27, 2016
        ///     Purpose:    Model class to interface with DB and feed data to 
        ///                 Controller
        /// </summary>
        private AppDbContext _db;
        /// <summary>
        /// constructor should pass instantiated DbContext
        /// <summary>
        public ItemModel(AppDbContext context)
        {
            _db = context;
        }
        public bool loadCategories(string rawJson)
        {
            bool loadedCategories = false;
            try
            {
                // clear out the old rows
                _db.Brands.RemoveRange(_db.Brands);
                _db.SaveChanges();

                dynamic decodedJson = Newtonsoft.Json.JsonConvert.DeserializeObject(rawJson);
                List<String> allBrands = new List<String>();

                foreach (var c in decodedJson)
                {
                    allBrands.Add(Convert.ToString(c["BRAND"]));  
                }

                // distinct will remove duplicates before we insert them into the db
                IEnumerable<String>brands = allBrands.Distinct<String>();

                foreach (string c in brands)
                {
                    Brand cat = new Brand();
                    cat.Name = c;
                    _db.Brands.Add(cat);
                    _db.SaveChanges();
                }
                loadedCategories = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedCategories;
        }
        public bool loadMenuItems(string rawJson)
        {
            bool loadedItems = false;
            try
            {
                List<Brand> brands = _db.Brands.ToList();
                // clear out the old
                _db.Items.RemoveRange(_db.Items);
                _db.SaveChanges();
                string decodedJsonStr = Decoder(rawJson);
                dynamic ItemJson = Newtonsoft.Json.JsonConvert.DeserializeObject(decodedJsonStr);
                foreach (var m in ItemJson)
                {
                    Item item = new Item();
                    item.Id = Convert.ToString(m["ID"]);
                    item.ProductName = Convert.ToString(m["PROD"]);
                    item.MSRP = Convert.ToDecimal(m["MSRP"]);
                    item.CostPrice = Convert.ToDecimal(m["COST"]);
                    item.GraphicName = Convert.ToString(m["GRPH"]);
                    item.QtyOnHand = Convert.ToInt32(m["HAND"]);
                    item.QtyOnBackOrder = Convert.ToInt32(m["BACK"]);
                    item.Description = Convert.ToString(m["DESC"]);
                    string brnd = Convert.ToString(m["BRAND"]);

                    foreach (Brand brand in brands)
                    {
                        if (brand.Name == brnd)
                            item.BrandId = brand.Id;
                    }

                    _db.Items.Add(item);
                    _db.SaveChanges();
                }
                loadedItems = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedItems;
        }
        public string Decoder(string value)
        {
            Regex regex = new Regex(@"\\u(?<Value>[a-zA-Z0-9]{4})", RegexOptions.Compiled);
            return regex.Replace(value, "");
        }
        public List<Item> GetAll()
        {
            return _db.Items.ToList();
        }
        public List<Item> GetAllByCategory(int id)
        {
            return _db.Items.Where(item => item.BrandId == id).ToList();
        }
    }
}
