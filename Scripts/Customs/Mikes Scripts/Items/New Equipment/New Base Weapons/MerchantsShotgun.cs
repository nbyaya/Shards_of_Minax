#region References
using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
#endregion

namespace Server.Items
{    
    /// <summary>
    /// A concrete example: MerchantsShotgun.
    /// This blaster uses SulfurousAsh as ammo and adds extra fire damage.
    /// </summary>
    [FlipableAttribute(0x13FD, 0x13FC)]
    public class MerchantsShotgun : BaseMagicRanged
    {
        [Constructable]
        public MerchantsShotgun() : base(0x13FD)
        {
            this.Weight = 9.0;
            this.Layer = Layer.TwoHanded;
			this.Name = "Merchants Shotgun";
			this.Hue = 755;
            // Additional initialization for a fire elemental blaster can be done here.
        }
        
        public MerchantsShotgun(Serial serial) : base(serial)
        {
        }
        
        // Use the same effect as the heavy crossbow (adjust if desired)
        public override int EffectID { get { return 0x36D4; } }
        
        // Define the reagent type used for this blaster.
        // (Ensure that the SulfurousAsh class exists in your project.)
        public override Type ReagentAmmoType { get { return typeof(SulfurousAsh); } }
        public override Item ReagentAmmo { get { return new SulfurousAsh(); } }
        
        // Override elemental damage properties – this variant adds fire damage.
        public override int FireDamage { get { return 20; } }
        
        // You can leave these abilities and damage values similar to the heavy crossbow,
        // or adjust them to suit your desired balance.
        public override WeaponAbility PrimaryAbility { get { return WeaponAbility.MovingShot; } }
        public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Dismount; } }
        public override int AosStrengthReq { get { return 80; } }
        public override int AosMinDamage { get { return 20; } }
        public override int AosMaxDamage { get { return 24; } }
        public override int AosSpeed { get { return 22; } }
        public override float MlSpeed { get { return 5.00f; } }
        public override int OldStrengthReq { get { return 40; } }
        public override int OldMinDamage { get { return 11; } }
        public override int OldMaxDamage { get { return 56; } }
        public override int OldSpeed { get { return 10; } }
        public override int DefMaxRange { get { return 8; } }
        public override int InitMinHits { get { return 31; } }
        public override int InitMaxHits { get { return 100; } }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Skill Required: ItemId");
			list.Add("Ammo: Sulfurous Ash");
        }	
		public override SkillName DefSkill
        {
            get
            {
                return SkillName.ItemID;
            }
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
