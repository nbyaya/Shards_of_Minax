using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an Abyssal Shadow corpse")]
    public class AbyssalShadow : BaseCreature
    {
        [Constructable]
        public AbyssalShadow() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Abyssal Shadow";
            Body = 0x190;            // You may change this to a body ID that fits your design.
            Hue = 0x455;             // A custom hue for a dark appearance.
            BaseSoundID = 0x482;     // Adjust the sound as needed.

            // Set Attributes
            SetStr(600, 700);
            SetDex(200, 300);
            SetInt(400, 500);

            // Set Damage and Damage Types
            SetDamage(20, 35);
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            // Set Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 40, 50);

            // Set Skills
            SetSkill(SkillName.EvalInt, 80.0, 100.0);
            SetSkill(SkillName.Magery, 90.0, 110.0);
            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.MagicResist, 85.0, 100.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 75.0, 90.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 60;
        }

        public override int TreasureMapLevel { get { return 4; } }
        public override bool BardImmune { get { return true; } }

        // The creature's AI loop
        public override void OnThink()
        {
            base.OnThink();

            // **Shadow Teleportation Ability:**  
            // When below 50% health, there’s a 5% chance every AI tick to teleport randomly within a 10-tile radius.
            if (!Deleted && Hits < HitsMax / 2 && Utility.RandomDouble() < 0.05)
            {
                TeleportRandom();
            }
        }

        private void TeleportRandom()
        {
            // Try up to 10 times to find a valid location
            Point3D originalLocation = this.Location;
            Map map = this.Map;
            if (map == null)
                return;

            for (int i = 0; i < 10; ++i)
            {
                int x = originalLocation.X + Utility.Random(21) - 10;
                int y = originalLocation.Y + Utility.Random(21) - 10;
                int z = map.GetAverageZ(x, y);
                Point3D newLoc = new Point3D(x, y, z);

                if (map.CanFit(newLoc, 16, false, false))
                {
                    // Visual effect at the original location
                    Effects.SendLocationParticles(EffectItem.Create(this.Location, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023, 2, 9953, 0);
                    
                    // Teleport the creature
                    this.Location = newLoc;
                    
                    // Visual effect at the new location
                    Effects.SendLocationParticles(EffectItem.Create(newLoc, map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023, 2, 9953, 0);
                    break;
                }
            }
        }

        // **Life Sapping Effect:**  
        // Occasionally, when damaged by a spell, the Abyssal Shadow drains a bit of life from the caster.
        public override void OnDamagedBySpell(Mobile from)
        {
            base.OnDamagedBySpell(from);

            if (Utility.RandomDouble() < 0.15 && from != null)
            {
                int drainAmount = Utility.RandomMinMax(10, 20);
                from.Damage(drainAmount);
                from.SendMessage("The Abyssal Shadow saps some of your life energy!");
            }
        }

        // **Death Burst Effect:**  
        // Just before death, the creature releases a burst of dark energy that damages nearby players.
        public override bool OnBeforeDeath()
        {
            if (!base.OnBeforeDeath())
                return false;

            // Display burst effect
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 30, 10, 0x480, 2, 9962, 0);

            // Damage all nearby players within 3 tiles
            foreach (Mobile m in this.GetMobilesInRange(3))
            {
                if (m != this && m.Alive && m.Player)
                {
                    int burstDamage = Utility.RandomMinMax(15, 25);
                    m.Damage(burstDamage, this);
                    m.SendMessage("The dying Abyssal Shadow releases a burst of dark energy, harming you!");
                }
            }
            return true;
        }

        public AbyssalShadow(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version number for future changes
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
