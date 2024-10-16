using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Vlad the Impaler")]
    public class UltimateMasterNecromancer : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterNecromancer()
            : base(AIType.AI_Mage)
        {
            Name = "Vlad the Impaler";
            Title = "The Ultimate Necromancer";
            Body = 0x190;
            Hue = 0x497;

            SetStr(400, 550);
            SetDex(80, 120);
            SetInt(600, 850);

            SetHits(15000);
            SetMana(3000);

            SetDamage(30, 45);

            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold, 30);
            SetDamageType(ResistanceType.Poison, 40);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.Necromancy, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 80;
			
            AddItem(new Robe(Utility.RandomRedHue()));
            AddItem(new Sandals());

            HairItemID = 0x203C; // Long Hair
            HairHue = 0x455;
        }

        public UltimateMasterNecromancer(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(BloodChalice), typeof(CloakOfShadows) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(NecromancerTome), typeof(BoneArmor) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(DarkCandle), typeof(NecromanticAltar) }; }
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

            c.DropItem(new PowerScroll(SkillName.Necromancy, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new BloodChalice());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new CloakOfShadows());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: BloodDrain(defender); break;
                    case 1: RaiseUndead(); break;
                    case 2: DarkPact(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void BloodDrain(Mobile defender)
        {
            if (defender != null)
            {
                DoHarmful(defender);

                int damage = Utility.RandomMinMax(40, 60);
                defender.Damage(damage, this);
                this.Hits += damage;

                defender.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Head);
                defender.PlaySound(0x1F2);
            }
        }

        public void RaiseUndead()
        {
            for (int i = 0; i < 3; ++i)
            {
                BaseCreature skeleton = new Skeleton();
                skeleton.Team = this.Team;
                skeleton.MoveToWorld(this.Location, this.Map);
                skeleton.Combatant = this.Combatant;
            }

            this.FixedParticles(0x375A, 10, 15, 5038, EffectLayer.Head);
            this.PlaySound(0x1EC);
        }

        public void DarkPact()
        {
            this.Hits -= 100;
            if (this.Combatant != null)
            {
                DoHarmful(this.Combatant);
                this.Combatant.Damage(200, this);

                this.Combatant.FixedParticles(0x374A, 10, 15, 5038, EffectLayer.Head);
                this.Combatant.PlaySound(0x1F2);
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

    public class BloodChalice : Item
    {
        [Constructable]
        public BloodChalice()
            : base(0x1F2C)
        {
            Name = "Blood Chalice";
            Hue = 0x485;
        }

        public BloodChalice(Serial serial) : base(serial)
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

    public class CloakOfShadows : BaseCloak
    {
        [Constructable]
        public CloakOfShadows() : base(0x1515)
        {
            Name = "Cloak of Shadows";
            Hue = 0x455;
        }

        public CloakOfShadows(Serial serial) : base(serial)
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
