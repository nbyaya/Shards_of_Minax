using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Sun Tzu")]
    public class UltimateMasterTactician : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterTactician()
            : base(AIType.AI_Melee)
        {
            Name = "Sun Tzu";
            Title = "The Master Strategist";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(505, 650);
            SetDex(152, 250);
            SetInt(305, 450);

            SetHits(15000);
            SetMana(2000);

            SetDamage(35, 45);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Fire, 10);
            SetDamageType(ResistanceType.Cold, 10);
            SetDamageType(ResistanceType.Energy, 5);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Tactics, 150.0);
            SetSkill(SkillName.Swords, 120.0);
            SetSkill(SkillName.Parry, 120.0);
            SetSkill(SkillName.Anatomy, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Healing, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 90;

            AddItem(new PlateChest());
            AddItem(new PlateArms());
            AddItem(new PlateLegs());
            AddItem(new PlateGloves());
            AddItem(new PlateGorget());
            AddItem(new Boots());
            AddItem(new Cloak(Utility.RandomRedHue()));

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x0;
        }

        public UltimateMasterTactician(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(ArtOfWarManual), typeof(StrategistsCloak) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(TacticsScroll), typeof(BattlePlans) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(WarBanner), typeof(AncientBattleMap) }; }
        }

        public override MonsterStatuetteType[] StatueTypes
        {
            get { return new MonsterStatuetteType[] { }; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 8);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new PowerScroll(SkillName.Tactics, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new ArtOfWarManual());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new StrategistsCloak());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: TacticalStrike(defender); break;
                    case 1: BattlefieldAwareness(); break;
                    case 2: WarCry(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void TacticalStrike(Mobile defender)
        {
            if (defender != null)
            {
                defender.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                defender.PlaySound(0x1FA);

                int damage = Utility.RandomMinMax(80, 100);
                AOS.Damage(defender, this, damage, 100, 0, 0, 0, 0);
                defender.SendLocalizedMessage(1070839); // You have been struck by a tactical blow!
            }
        }

        public void BattlefieldAwareness()
        {
            ArrayList allies = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(10))
            {
                if (m != this && m.Player && this.CanBeBeneficial(m))
                    allies.Add(m);
            }

            for (int i = 0; i < allies.Count; ++i)
            {
                Mobile m = (Mobile)allies[i];

                DoBeneficial(m);

                m.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                m.PlaySound(0x1F7);

                m.SendLocalizedMessage(1070841); // You feel more aware of the battlefield!

                m.VirtualArmorMod += 20;
                Timer.DelayCall(TimeSpan.FromSeconds(30.0), delegate { m.VirtualArmorMod -= 20; });
            }
        }

        public void WarCry()
        {
            ArrayList allies = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(10))
            {
                if (m != this && m.Player && this.CanBeBeneficial(m))
                    allies.Add(m);
            }

            for (int i = 0; i < allies.Count; ++i)
            {
                Mobile m = (Mobile)allies[i];

                DoBeneficial(m);

                m.FixedParticles(0x36B0, 1, 30, 0x26BD, 0x3F, 0x7, EffectLayer.Head);
                m.PlaySound(0x1F8);

                m.SendLocalizedMessage(1070842); // You feel empowered by the war cry!

                m.Str += 20;
                m.Dex += 20;
                Timer.DelayCall(TimeSpan.FromSeconds(30.0), delegate { m.Str -= 20; m.Dex -= 20; });
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

    public class ArtOfWarManual : Item
    {
        [Constructable]
        public ArtOfWarManual() : base(0x1C10)
        {
            Name = "Art of War Manual";
            Hue = 0x48D;
            Weight = 1.0;
        }

        public ArtOfWarManual(Serial serial) : base(serial)
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

    public class StrategistsCloak : Item
    {
        [Constructable]
        public StrategistsCloak() : base(0x1515)
        {
            Name = "Strategist's Cloak";
            Hue = 0x455;
            Weight = 5.0;
        }

        public StrategistsCloak(Serial serial) : base(serial)
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
