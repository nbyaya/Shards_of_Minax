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
    /// Base class for Alchemy Blasters.
    /// Uses the Alchemy skill and consumes reagents as ammo.
    /// </summary>
    public abstract class BaseMagicRanged : BaseRanged
    {
        // Use Alchemy instead of Archery.
        public override SkillName DefSkill { get { return SkillName.Alchemy; } }
        
        // Abstract properties to define which reagent is used as ammo.
        public abstract Type ReagentAmmoType { get; }
        public abstract Item ReagentAmmo { get; }

		public override int DefHitSound { get { return 0x15E; } }
		public override int DefMissSound { get { return 0x15E; } }
        
        // For compatibility, override AmmoType and Ammo to return the reagent properties.
        public override Type AmmoType { get { return ReagentAmmoType; } }
        public override Item Ammo { get { return ReagentAmmo; } }
        
        // Additional elemental damage properties can be defined and overridden by derived classes.
        public virtual int FireDamage { get { return 0; } }
        public virtual int ColdDamage { get { return 0; } }
        public virtual int PoisonDamage { get { return 0; } }
        public virtual int EnergyDamage { get { return 0; } }
        
        public BaseMagicRanged(int itemID) : base(itemID)
        {
            // Optionally set default properties here
        }
        
        public BaseMagicRanged(Serial serial) : base(serial)
        {
        }
        
        // Override OnFired to consume reagents from the backpack.
        public override bool OnFired(Mobile attacker, IDamageable damageable)
        {
            if (attacker.Player)
            {
                Container pack = attacker.Backpack;
                // Try to consume one unit of the reagent
                if (pack == null || !pack.ConsumeTotal(ReagentAmmoType, 1))
                {
                    attacker.SendMessage("You do not have the required reagent to fire this weapon.");
                    return false;
                }
            }
            
            // Play the moving effect (using the heavy crossbow animation)
            attacker.MovingEffect(damageable, EffectID, 18, 1, false, false);
            return true;
        }
        
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            // Write additional properties if needed.
        }
        
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            // Read additional properties if needed.
        }
    }    
}
