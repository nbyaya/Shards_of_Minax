using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Mahatma Gandhi")]
    public class UltimateMasterPeacemaker : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterPeacemaker()
            : base(AIType.AI_Mage)
        {
            Name = "Mahatma Gandhi";
            Title = "The Ultimate Peacemaker";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(12000);
            SetMana(2500);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Peacemaking, 120.0);

            Fame = 22500;
            Karma = 22500;

            VirtualArmor = 70;
            
            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new Sandals());

            HairItemID = 0x204B; // Bald
            HairHue = 0x47E;

            FacialHairItemID = 0x204D; // Goatee
            FacialHairHue = 0x47E;
        }

        public UltimateMasterPeacemaker(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(PeacekeepersStaff), typeof(RobesOfPeace) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(PeacemakersManual), typeof(CalmPotion) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(PeacekeepersStaff), typeof(PeaceLily) }; }
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

            c.DropItem(new PowerScroll(SkillName.Peacemaking, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new PeacekeepersStaff());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new RobesOfPeace());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: Pacify(); break;
                    case 1: TranquilAura(); break;
                    case 2: Harmony(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void Pacify()
        {
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

                m.SendMessage("You feel a wave of peace wash over you, reducing your attack power.");

                // Reduce attack power by 50% for 10 seconds
                m.BeginAction(typeof(UltimateMasterPeacemaker));
                Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerStateCallback(EndPacify), m);
            }
        }

        private void EndPacify(object state)
        {
            Mobile m = (Mobile)state;

            if (m != null)
                m.EndAction(typeof(UltimateMasterPeacemaker));
        }

        public void TranquilAura()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeBeneficial(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoBeneficial(m);

                m.SendMessage("You feel a soothing aura healing your wounds.");

                // Heal 50 hit points over 5 seconds
                m.Hits += 50;
            }
        }

        public void Harmony()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeBeneficial(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoBeneficial(m);

                m.SendMessage("You feel a sense of harmony boosting your resistances and defenses.");

                // Buff resistances and defenses by 10% for 15 seconds
                m.VirtualArmorMod += 10;
                Timer.DelayCall(TimeSpan.FromSeconds(15.0), new TimerStateCallback(EndHarmony), m);
            }
        }

        private void EndHarmony(object state)
        {
            Mobile m = (Mobile)state;

            if (m != null)
                m.VirtualArmorMod -= 10;
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

    public class PeacekeepersStaff : Item
    {
        [Constructable]
        public PeacekeepersStaff()
            : base(0xDF0)
        {
            Weight = 6.0;
            Hue = 0x482;
            Name = "Peacekeeper's Staff";
        }

        public PeacekeepersStaff(Serial serial)
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

    public class RobesOfPeace : BaseClothing
    {
        [Constructable]
        public RobesOfPeace()
            : base(0x1F04, Layer.OuterTorso, 0)
        {
            Weight = 3.0;
            Hue = 0x47E;
            Name = "Robes of Peace";
        }

        public RobesOfPeace(Serial serial)
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
