using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Arsène Lupin")]
    public class UltimateMasterLockpicker : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterLockpicker()
            : base(AIType.AI_Melee)
        {
            Name = "Arsène Lupin";
            Title = "The Master Thief";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(305, 425);
            SetDex(205, 325);
            SetInt(505, 750);

            SetHits(12000);
            SetMana(2500);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Lockpicking, 120.0);
            SetSkill(SkillName.Stealth, 120.0);
            SetSkill(SkillName.Hiding, 120.0);
            SetSkill(SkillName.Snooping, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            AddItem(new Shirt(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomNeutralHue()));
            AddItem(new Boots());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x44E;
        }

        public UltimateMasterLockpicker(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(MasterThiefToolkit), typeof(ShadowCloak) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(LockpickSet), typeof(SkeletonKey) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(MasterThiefToolkit), typeof(TreasureBox) }; }
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

            c.DropItem(new PowerScroll(SkillName.Lockpicking, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MasterThiefToolkit());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new ShadowCloak());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: SilentEntry(); break;
                    case 1: TrapDisarm(); break;
                    case 2: TreasureSense(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void SilentEntry()
        {
            // Implementation of Silent Entry ability
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoHarmful(m);

                int damage = Utility.RandomMinMax(40, 60);

                AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);

                m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                m.PlaySound(0x207);
            }
        }

        public void TrapDisarm()
        {
            // Implementation of Trap Disarm ability
            ArrayList traps = new ArrayList();

            foreach (Item item in this.GetItemsInRange(8))
            {
                if (item is BaseTrap)
                    traps.Add(item);
            }

            for (int i = 0; i < traps.Count; ++i)
            {
                BaseTrap trap = (BaseTrap)traps[i];
                trap.Delete();

                FixedParticles(0x36B0, 1, 14, 0x26B8, 0x3F, 0x7, EffectLayer.Head);
                PlaySound(0x229);
            }
        }

        public void TreasureSense()
        {
            // Implementation of Treasure Sense ability
            ArrayList treasures = new ArrayList();

            foreach (Item item in this.GetItemsInRange(8))
            {
                if (item is TreasureBox)
                    treasures.Add(item);
            }

            for (int i = 0; i < treasures.Count; ++i)
            {
                TreasureBox treasure = (TreasureBox)treasures[i];
                treasure.Visible = true;

                FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                PlaySound(0x1FA);
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

    public class MasterThiefToolkit : Item
    {
        [Constructable]
        public MasterThiefToolkit()
            : base(0x1EB8)
        {
            Hue = 0x455;
            Name = "Master Thief's Toolkit";
        }

        public MasterThiefToolkit(Serial serial)
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

    public class ShadowCloak : BaseCloak
    {
        [Constructable]
        public ShadowCloak()
            : base(0x1515)
        {
            Hue = 0x455;
            Name = "Shadow Cloak";
        }

        public ShadowCloak(Serial serial)
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

    public class LockpickSet : Item
    {
        [Constructable]
        public LockpickSet()
            : base(0x14FC)
        {
            Hue = 0x455;
            Name = "Lockpick Set";
        }

        public LockpickSet(Serial serial)
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

    public class SkeletonKey : Item
    {
        [Constructable]
        public SkeletonKey()
            : base(0x100E)
        {
            Hue = 0x455;
            Name = "Skeleton Key";
        }

        public SkeletonKey(Serial serial)
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

    public class TreasureBox : LockableContainer
    {
        [Constructable]
        public TreasureBox()
            : base(0xE41)
        {
            Hue = 0x455;
            Name = "Treasure Chest";
            Movable = false;
        }

        public TreasureBox(Serial serial)
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
