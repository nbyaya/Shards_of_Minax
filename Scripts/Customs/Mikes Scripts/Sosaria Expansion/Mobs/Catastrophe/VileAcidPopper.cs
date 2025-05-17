using System;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a vile acid popper corpse")]
    public class VileAcidPopper : BaseCreature
    {
        [Constructable]
        public VileAcidPopper()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Vile Acid Popper";
            Body = 0x2808;
            BaseSoundID = 0x3E; // Acid pop sound
            Hue = 0xB94; // Sickly green-purple hue

            SetStr(150, 200);
            SetDex(100, 120);
            SetInt(120, 140);

            SetHits(300, 350);
            SetStam(150, 180);
            SetMana(250, 300);

            SetDamage(12, 18);
            SetDamageType(ResistanceType.Poison, 80);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Magery, 80.0, 100.0);
            SetSkill(SkillName.EvalInt, 80.0, 100.0);
            SetSkill(SkillName.Poisoning, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 70.0, 80.0);
            SetSkill(SkillName.Wrestling, 75.0, 90.0);

            Fame = 6000;
            Karma = -6000;
            VirtualArmor = 25;
            PackItem(new GreaterPoisonPotion());
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Deadly;
        public override Poison HitPoison => Poison.Greater;

        private DateTime _nextAcidBurst;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target && DateTime.UtcNow > _nextAcidBurst && InRange(target, 10))
            {
                AcidBurst(target);
                _nextAcidBurst = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
            }
        }

        private void AcidBurst(Mobile target)
        {
            PublicOverheadMessage(MessageType.Emote, 0x3F, false, "*hisses violently and swells*");

            Effects.SendLocationParticles(EffectItem.Create(Location, Map, TimeSpan.FromSeconds(0.5)), 0x374A, 20, 30, 0x3F, 0);
            PlaySound(0x22F);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m == null || !CanBeHarmful(m) || m == this)
                    continue;

                DoHarmful(m);

                int damage = Utility.RandomMinMax(20, 30);
                AOS.Damage(m, this, damage, 0, 0, 0, 0, 100); // 100% Energy (acidic splash)

                if (Utility.RandomDouble() < 0.5)
                    m.ApplyPoison(this, Poison.Regular);

                m.SendAsciiMessage("You are splashed by sizzling acid!");
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (defender != null && Combatant is Mobile target)
            {
                if (Utility.RandomDouble() < 0.3)
                {
                    defender.SendAsciiMessage("The Vile Acid Popper's wound releases acidic gas!");
                    defender.PlaySound(0x231);
                    Effects.SendLocationParticles(EffectItem.Create(defender.Location, defender.Map, TimeSpan.FromSeconds(0.5)), 0x3709, 10, 15, 0x3F, 0, 5022, 0);

                    Timer.DelayCall(TimeSpan.FromSeconds(1.5), () =>
                    {
                        if (!defender.Deleted && defender.Alive)
                        {
                            AOS.Damage(defender, this, Utility.RandomMinMax(10, 20), 0, 0, 0, 0, 100);
                            if (Utility.RandomBool())
                                defender.ApplyPoison(this, Poison.Lesser);
                        }
                    });
                }
            }
        }

		public override bool OnBeforeDeath()
		{
			PublicOverheadMessage(MessageType.Emote, 0x3F, false, "*ruptures in a boiling cascade of acid!*");
			PlaySound(0x307);

			foreach (Mobile m in GetMobilesInRange(4))
			{
				if (m != null && CanBeHarmful(m) && m != this)
				{
					DoHarmful(m);
					AOS.Damage(m, this, Utility.RandomMinMax(25, 40), 0, 0, 0, 0, 100);

					if (Utility.RandomDouble() < 0.4)
						m.ApplyPoison(this, Poison.Greater);

					m.SendAsciiMessage("You are doused in corrosive bile as the creature dies!");
				}
			}

			return base.OnBeforeDeath(); // this ensures the creature still dies
		}


        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 3));
        }

        public override int TreasureMapLevel => 2;

        public VileAcidPopper(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
