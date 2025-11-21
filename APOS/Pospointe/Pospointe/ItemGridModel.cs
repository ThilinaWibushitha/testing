using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pospointe
{
    public class ItemGridModel : INotifyPropertyChanged
    {
        private Guid _uniqueId = Guid.NewGuid();
        private int _orderNo;
        private string _itemName;
        private string _itemId;
        private string _deptId;
        private decimal _price;
        private int _quantity;
        private decimal _totalPrice;
        private decimal _discount = 0;
        private bool _isModifier;
        private Guid _parentItemId;
        private bool _isTax1;
        private bool _isVoided;
        private bool _isDoubleClickable = true;
        private bool _isKOT1 = false;
        private bool _isKOT2 = false;

        public Guid UniqueId
        {
            get => _uniqueId;
            set => SetField(ref _uniqueId, value);
        }

        public int OrderNo
        {
            get => _orderNo;
            set => SetField(ref _orderNo, value);
        }

        public string ItemName
        {
            get => _itemName;
            set => SetField(ref _itemName, value);
        }

        public string Itemid
        {
            get => _itemId;
            set => SetField(ref _itemId, value);
        }

        public string Deptid
        {
            get => _deptId;
            set => SetField(ref _deptId, value);
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (SetField(ref _price, value))
                    TotalPrice = _price * Quantity; // auto-update total
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (SetField(ref _quantity, value))
                    TotalPrice = _price * _quantity; // auto-update total
            }
        }

        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetField(ref _totalPrice, value);
        }

        public decimal Discount
        {
            get => _discount;
            set => SetField(ref _discount, value);
        }

        public bool IsModifier
        {
            get => _isModifier;
            set => SetField(ref _isModifier, value);
        }

        public Guid ParentItemId
        {
            get => _parentItemId;
            set => SetField(ref _parentItemId, value);
        }

        public bool isTax1
        {
            get => _isTax1;
            set => SetField(ref _isTax1, value);
        }

        public bool IsVoided
        {
            get => _isVoided;
            set => SetField(ref _isVoided, value);
        }

        public bool IsDoubleClickable
        {
            get => _isDoubleClickable;
            set => SetField(ref _isDoubleClickable, value);
        }

        public bool IsKOT1
        {
            get => _isKOT1;
            set => SetField(ref _isKOT1, value);
        }

        public bool IsKOT2
        {
            get => _isKOT2;
            set => SetField(ref _isKOT2, value);
        }

        // === INotifyPropertyChanged implementation ===
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}
