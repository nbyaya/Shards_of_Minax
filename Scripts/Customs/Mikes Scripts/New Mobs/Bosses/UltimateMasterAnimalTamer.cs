using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Steve Irwin")]
    public class UltimateMasterAnimalTamer : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterAnimalTamer()
            : base(AIType.AI_Melee)
        {
            Name = "Steve Irwin";
            Title = "The Crocodile Hunter";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(400, 500);
            SetDex(150, 200);
            SetInt(200, 300);

            SetHits(15000);
            SetMana(1500);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.AnimalTaming, 120.0);
            SetSkill(SkillName.AnimalLore, 120.0);
            SetSkill(SkillName.Veterinary, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 25000;
            Karma = 25000;

            VirtualArmor = 80;
            
            AddItem(new LongPants(Utility.RandomYellowHue()));
			AddItem(new WideBrimHat(Utility.RandomYellowHue()));
            AddItem(new Boots());

            HairItemID = 0x203C; // Short hair
            HairHue = 0x47E;
        }

        public UltimateMasterAnimalTamer(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(TamingWhip), typeof(CrocodileToothNecklace) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(TamerGloves), typeof(VeterinaryTools) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(StuffedCrocodile), typeof(AnimalTamingBook) }; }
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

            c.DropItem(new PowerScroll(SkillName.AnimalTaming, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new TamingWhip());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new CrocodileToothNecklace());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: WildCommand(); break;
                    case 1: CrocodileSpin(); break;
                    case 2: VenomousBite(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void WildCommand()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(10))
            {
                if (m is BaseCreature && m != this && ((BaseCreature)m).Controlled && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoHarmful(m);

                ((BaseCreature)m).ControlMaster = this;
                ((BaseCreature)m).ControlOrder = OrderType.Attack;
                ((BaseCreature)m).ControlTarget = this.Combatant;

                m.FixedParticles(0x3728, 10, 15, 5038, EffectLayer.Head);
                m.PlaySound(0x209);
            }
        }

        public void CrocodileSpin()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(5))
            {
                if (m != this && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoHarmful(m);

                AOS.Damage(m, this, Utility.RandomMinMax(50, 70), 100, 0, 0, 0, 0);

                m.FixedParticles(0x3728, 10, 15, 5038, EffectLayer.Waist);
                m.PlaySound(0x213);
            }
        }

        public void VenomousBite(Mobile defender)
        {
            if (defender != null)
            {
                defender.FixedParticles(0x3728, 10, 15, 5038, EffectLayer.Waist);
                defender.PlaySound(0x1FA);

                defender.ApplyPoison(this, Poison.Deadly);

                defender.SendLocalizedMessage(1070722); // You have been poisoned by a venomous bite!
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

    // Custom items
    public class TamingWhip : Item
    {
        [Constructable]
        public TamingWhip() : base(0x166E)
        {
            Name = "Taming Whip";
            Hue = 1153;
        }

        public TamingWhip(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class CrocodileToothNecklace : Item
    {
        [Constructable]
        public CrocodileToothNecklace() : base(0x1088)
        {
            Name = "Crocodile Tooth Necklace";
            Hue = 1157;
        }

        public CrocodileToothNecklace(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
