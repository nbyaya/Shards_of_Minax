using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;  // for SpellHelper, if you ever use it
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a marrowth corpse")]
    public class Marrowth : BaseCreature
    {
        private DateTime _NextBreathAttack;
        private DateTime _NextBoneSpike;

        [Constructable]
        public Marrowth()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Marrowth";
            Body = 104;
            BaseSoundID = 0x488;
            Hue = 0x455;

            SetStr(900, 1100);
            SetDex(150, 200);
            SetInt(600, 800);

            SetHits(1500, 2000);

            SetDamage(35, 45);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold,     50);
            SetDamageType(ResistanceType.Poison,    0);

            SetResistance(ResistanceType.Physical, 80, 85);
            SetResistance(ResistanceType.Fire,     50, 60);
            SetResistance(ResistanceType.Cold,     60, 70);
            SetResistance(ResistanceType.Poison,   25, 35);
            SetResistance(ResistanceType.Energy,   50, 60);

            SetSkill(SkillName.EvalInt,    100.0, 120.0);
            SetSkill(SkillName.Magery,     100.0, 120.0);
            SetSkill(SkillName.MagicResist,110.0, 130.0);
            SetSkill(SkillName.Tactics,    100.0, 120.0);
            SetSkill(SkillName.Wrestling,  100.0, 120.0);
            SetSkill(SkillName.Necromancy, 100.0, 120.0);
            SetSkill(SkillName.SpiritSpeak,100.0, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 90;

            _NextBreathAttack = DateTime.UtcNow;
            _NextBoneSpike   = DateTime.UtcNow;
        }

        public Marrowth(Serial serial) : base(serial) { }

        public override bool AutoDispel            => true;
        public override bool BleedImmune          => true;
        public override bool ReacquireOnMovement  => true;
        public override int  Hides                => 30;
        public override HideType HideType         => HideType.Barbed;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;
        public override Poison PoisonImmune       => Poison.Lethal;
        public override TribeType Tribe           => TribeType.Undead;
        public override bool Unprovokable         => true;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= _NextBreathAttack && InRange(target, 10) && CanBeHarmful(target) && InLOS(target))
                {
                    BreathAttack(target);
                    _NextBreathAttack = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
                }

                if (DateTime.UtcNow >= _NextBoneSpike && InRange(target, 15) && CanBeHarmful(target) && InLOS(target))
                {
                    BoneSpike(target);
                    _NextBoneSpike   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
                }
            }
        }

        public void BreathAttack(Mobile target)
        {
            Animate(10, 5, 1, true, false, 0);
            Say("Feel the chill of the grave!");

            Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
            {
                if (target == null || target.Deleted || !InRange(target, 10) || !CanBeHarmful(target) || !InLOS(target))
                    return;

                DoHarmful(target);

                var eable = GetMobilesInRange(6);
                foreach (Mobile m in eable)
                {
                    if (m == this || !CanBeHarmful(m)) continue;

                    DoHarmful(m);

                    // ** FIXED: use the 13‑param overload – no EffectLayer enum here **
                    Effects.SendMovingParticles(
                        new Entity(Serial.Zero, new Point3D(X, Y, Z + 16), Map),
                        new Entity(Serial.Zero, new Point3D(m.X, m.Y, m.Z + 16), Map),
                        0x36D4,   // itemID
                        7, 0,     // speed, duration
                        false, true,  // fixedDir, explodes
                        0x481, 0,    // hue, renderMode
                        9502, 6014,  // effect, explodeEffect
                        0x11D,       // explodeSound
                        0            // unknown/trailing
                    );

                    m.PlaySound(0x23E);

                    int coldDamage = Utility.RandomMinMax(40, 60);
                    AOS.Damage(m, this, coldDamage, 0, 0, 100, 0, 0);

                    if (Utility.RandomDouble() < 0.5)
                    {
                        m.Stam -= Utility.RandomMinMax(10, 20);
                        m.Mana -= Utility.RandomMinMax(10, 20);
                        m.SendAsciiMessage("You feel the life drain from you!");
                    }
                }
                eable.Free();
            });
        }

        public void BoneSpike(Mobile target)
        {
            Animate(11, 5, 1, true, false, 0);
            Say("Impaled!");

            Timer.DelayCall(TimeSpan.FromSeconds(1.5), () =>
            {
                if (target == null || target.Deleted || !InRange(target, 15) || !CanBeHarmful(target) || !InLOS(target))
                    return;

                DoHarmful(target);

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, new Point3D(X, Y, Z + 16), Map),
                    new Entity(Serial.Zero, new Point3D(target.X, target.Y, target.Z + 16), Map),
                    0x2A76,   // itemID
                    7, 0,     // speed, duration
                    false, false, // fixedDir, explodes
                    0, 0,        // hue, renderMode
                    9502, 6014,  // effect, explodeEffect
                    0x11D,       // explodeSound
                    0            // unknown/trailing
                );

                Timer.DelayCall(TimeSpan.FromSeconds(0.7), () =>
                {
                    if (target == null || target.Deleted || !InRange(target, 1) || !CanBeHarmful(target))
                        return;

                    target.PlaySound(0x215);

                    int physDamage = Utility.RandomMinMax(25,35);
                    AOS.Damage(target, this, physDamage, 100,0,0,0,0);

                    if (Utility.RandomDouble() < 0.3)
                    {
                        BleedAttack.BeginBleed(target, this);
                        target.SendAsciiMessage("You are bleeding!");
                    }
                });
            });
        }

		public override void OnGaveMeleeAttack(Mobile defender)
		{
			base.OnGaveMeleeAttack(defender);

			if (Utility.RandomDouble() < 0.2 && defender is Mobile m)
			{
				var skillToDebuff = Utility.RandomBool() ? SkillName.Tactics : SkillName.Wrestling;
				double reduction = Utility.RandomMinMax(5.0, 10.0);

				var mod = new DefaultSkillMod(skillToDebuff, true, -reduction);
				m.AddSkillMod(mod);

				// remove it in 10 seconds
				Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
				{
					m.RemoveSkillMod(mod); // FIX: use the actual mod object
				});

				m.SendAsciiMessage($"Marrowth's attack has reduced your {skillToDebuff} skill!");
			}
		}


        public override bool CanRummageCorpses => true;
        public override int  TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.HighScrolls, 2);
            AddLoot(LootPack.Gems, 8);

            // If you want to drop an artifact, you could do:
            // if (Utility.RandomDouble() < 0.05)
            //     PackItem(Loot.RandomArtifact());
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
