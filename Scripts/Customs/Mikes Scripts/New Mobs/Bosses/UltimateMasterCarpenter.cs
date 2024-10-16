using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of Geppetto")]
    public class UltimateMasterCarpenter : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterCarpenter()
            : base(AIType.AI_Melee)
        {
            Name = "Geppetto";
            Title = "The Master Carpenter";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(12000);
            SetMana(2500);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Carpentry, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            AddItem(new Shirt());
            AddItem(new LongPants());
            AddItem(new Boots());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x44E;
        }

        public UltimateMasterCarpenter(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(MasterCarpentersSaw), typeof(WoodenPuppet) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(Log), typeof(Board) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(WoodenBench), typeof(WoodenChair) }; }
        }

        public override MonsterStatuetteType[] StatueTypes
        {
            get { return new MonsterStatuetteType[] { }; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 6);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new PowerScroll(SkillName.Carpentry, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MasterCarpentersSaw());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new WoodPuppet());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: WoodenBarrier(); break;
                    case 1: PuppetControl(); break;
                    case 2: SplinterStrike(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void WoodenBarrier()
        {
            // Create temporary wooden barriers that block movement and provide cover

        }

        public void PuppetControl()
        {
            // Summon wooden puppets to fight for Geppetto
            for (int i = 0; i < 3; i++)
            {
                WoodenPuppet puppet = new WoodenPuppet();
                puppet.Team = this.Team;
                puppet.MoveToWorld(this.Location, this.Map);
                puppet.Combatant = this.Combatant;
            }
        }

        public void SplinterStrike(Mobile defender)
        {
            if (defender != null)
            {
                defender.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                defender.PlaySound(0x1FA);

                int damage = Utility.RandomMinMax(60, 80);
                AOS.Damage(defender, this, damage, 100, 0, 0, 0, 0);

                // Apply bleeding damage over time
                defender.SendLocalizedMessage(1070839); // You are bleeding from the splinter strike!
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

    public class WoodenBarrier : Item
    {
        public Timer Timer;

        [Constructable]
        public WoodenBarrier() : base(0x1B7D) // Use a wooden barrier item ID
        {
            Movable = false;
        }

        public WoodenBarrier(Serial serial) : base(serial)
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

    public class WoodenPuppet : BaseCreature
    {
        [Constructable]
        public WoodenPuppet() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a wooden puppet";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(200, 300);
            SetDex(100, 150);
            SetInt(50, 75);

            SetHits(500);
            SetMana(0);

            SetDamage(10, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 50.0);
            SetSkill(SkillName.Tactics, 60.0);
            SetSkill(SkillName.Wrestling, 60.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 25;
        }

        public WoodenPuppet(Serial serial) : base(serial)
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
