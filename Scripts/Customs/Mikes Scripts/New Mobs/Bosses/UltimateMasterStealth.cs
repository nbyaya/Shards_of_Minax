using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Silent Snake")]
    public class UltimateMasterStealth : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterStealth()
            : base(AIType.AI_Thief)
        {
            Name = "Silent Snake";
            Title = "The Master of Stealth";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(250, 350);
            SetDex(300, 450);
            SetInt(200, 300);

            SetHits(10000);
            SetStam(2500);
            SetMana(1500);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Stealth, 120.0);
            SetSkill(SkillName.Hiding, 120.0);
            SetSkill(SkillName.Fencing, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Poisoning, 120.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 60;
			
            AddItem(new LeatherChest());
            AddItem(new LeatherLegs());
            AddItem(new LeatherArms());
            AddItem(new LeatherGloves());
            AddItem(new LeatherGorget());
            AddItem(new Boots());
            
            HairItemID = 0x203B; // Short Hair
            HairHue = 0x44E;
        }

        public UltimateMasterStealth(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(ShadowArmor), typeof(SmokeBoomb) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(ShadowArmor), typeof(StealthCloak) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(ShadowStatue), typeof(StealthDagger) }; }
        }

        public override MonsterStatuetteType[] StatueTypes
        {
            get { return new MonsterStatuetteType[] { }; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 5);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new PowerScroll(SkillName.Stealth, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MaxxiaScroll());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MaxxiaScroll());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: Vanish(); break;
                    case 1: SilentKill(defender); break;
                    case 2: Evasion(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
        }

        public void Vanish()
        {
            this.Hidden = true;
            this.Say("You can't see me!");
            this.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Head);
            this.PlaySound(0x1FD);
        }

        public void SilentKill(Mobile defender)
        {
            if (defender != null)
            {
                this.DoHarmful(defender);
                int damage = Utility.RandomMinMax(50, 70);

                AOS.Damage(defender, this, damage, 100, 0, 0, 0, 0);

                defender.FixedParticles(0x37B9, 10, 15, 5044, EffectLayer.Head);
                defender.PlaySound(0x208);
            }
        }

        public void Evasion()
        {
            this.SendMessage("Silent Snake evades!");
            this.VirtualArmor += 20;
            this.FixedParticles(0x376A, 10, 15, 5037, EffectLayer.Waist);
            this.PlaySound(0x1F2);
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
