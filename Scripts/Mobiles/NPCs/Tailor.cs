using System;
using System.Collections.Generic;
using Server.Engines.BulkOrders;

namespace Server.Mobiles
{
    public class Tailor : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos
        {
            get
            {
                return this.m_SBInfos;
            }
        }
		
		public override bool UsesRandomisedStock => true;

        public override NpcGuild NpcGuild
        {
            get
            {
                return NpcGuild.TailorsGuild;
            }
        }

        [Constructable]
        public Tailor()
            : base("the tailor")
        {
            this.SetSkill(SkillName.Tailoring, 64.0, 100.0);
        }

        public override void InitSBInfo()
        {
            if(!IsStygianVendor)
            { 
                this.m_SBInfos.Add(new SBTailor());
            }
            else
            {
                this.m_SBInfos.Add(new SBSATailor());
            }
        }

        public override VendorShoeType ShoeType
        {
            get
            {
                return Utility.RandomBool() ? VendorShoeType.Sandals : VendorShoeType.Shoes;
            }
        }

        #region Bulk Orders
        public override BODType BODType { get { return BODType.Tailor; } }

        public override Item CreateBulkOrder(Mobile from, bool fromContextMenu)
        {
            PlayerMobile pm = from as PlayerMobile;

            if (pm != null && pm.NextTailorBulkOrder == TimeSpan.Zero && (fromContextMenu || 0.2 > Utility.RandomDouble()))
            {
                double theirSkill = pm.Skills[SkillName.Tailoring].Base;

                if (theirSkill >= 70.1)
                    pm.NextTailorBulkOrder = TimeSpan.FromHours(6.0);
                else if (theirSkill >= 50.1)
                    pm.NextTailorBulkOrder = TimeSpan.FromHours(2.0);
                else
                    pm.NextTailorBulkOrder = TimeSpan.FromHours(1.0);

                if (theirSkill >= 70.1 && ((theirSkill - 40.0) / 300.0) > Utility.RandomDouble())
                    return new LargeTailorBOD();

                return SmallTailorBOD.CreateRandomFor(from);
            }

            return null;
        }

        public override bool IsValidBulkOrder(Item item)
        {
            return (item is SmallTailorBOD || item is LargeTailorBOD);
        }

        public override bool SupportsBulkOrders(Mobile from)
        {
            return (from is PlayerMobile && from.Skills[SkillName.Tailoring].Base > 0);
        }

        public override TimeSpan GetNextBulkOrder(Mobile from)
        {
            if (from is PlayerMobile)
                return ((PlayerMobile)from).NextTailorBulkOrder;

            return TimeSpan.Zero;
        }

        public override void OnSuccessfulBulkOrderReceive(Mobile from)
        {
            if (Core.SE && from is PlayerMobile)
                ((PlayerMobile)from).NextTailorBulkOrder = TimeSpan.Zero;
        }

        #endregion

        public Tailor(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}